using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Thuleanx;
using Thuleanx.Optimization;

enum MODE {
	BEGIN,
	PLAYER_TURN,
	OTHER_TURN,
	NATURE_TURN,
	END
}

public class GameMaster : MonoBehaviour {
	MODE mode = MODE.BEGIN;
	Grid grid;

	public int board_width = 8, board_height = 8;

	public BubblePool pawn, zealot;
	public BubblePool moveIndicator, actionIndicator;
	public UnitMold Soldier, Healer, Scout;

	// public BubblePool boardBase;
	// public BubblePool boardBaseAlternative;

	void Start() {
		StartGame();
	}

	public void StartGame() => StartCoroutine(_StartGame());
	public void PlayerTurn() => StartCoroutine(_PlayerTurn());
	public void OtherTurn() => StartCoroutine(_OtherTurn());

	public void SpawnTestUnitAt(Vector2Int position, CharacterClass charClass) {
		PlayableUnit punit;
		switch (charClass) {
			case CharacterClass.Soldier:
				punit = Soldier.Create(position, grid.GetPosCenter(position));
				break;
			case CharacterClass.Healer:
				punit = Healer.Create(position, grid.GetPosCenter(position));
				break;
			case CharacterClass.Scout:
				punit = Scout.Create(position, grid.GetPosCenter(position));
				break;
			default:
				punit = Soldier.Create(position, grid.GetPosCenter(position));
				break;
		}
		grid.AddOccupant(position, punit);
		App.LocalInstance._VisionTracker.AddUnit(punit);
		App.LocalInstance._UIManager.SidebarManager.AddUnit(punit);
	}
	public void SpawnTestZealotAt(Vector2Int position) {
		Zealot unit = new Zealot(position);
		GameObject pawnObj = zealot.Borrow(grid.GetPosCenter(unit.position), Quaternion.identity);
		unit.range = 2;
		unit.Attach(pawnObj);
		grid.AddOccupant(unit.position, unit);
	}

	#region Player Turn Variables
	List<GameObject> _indicators = new List<GameObject>();
	Unit _selected_unit = null;
	#endregion

	public void WipeIndicators() {
		foreach (GameObject obj in _indicators) obj.SetActive(false);
		_indicators.Clear();
	}

	IEnumerator _StartGame() {
		mode = MODE.BEGIN;

		// construct the grid logically
		grid = new Grid(board_width, board_height, transform.position, 1f);
		// // instantiating the grid 
		// foreach (Cell cell in grid.arr) {
		// 	if ((cell.x + cell.y)%2 == 0)	boardBase.Borrow(grid.GetPosCenter(cell.position), Quaternion.identity);
		// 	else 							boardBaseAlternative.Borrow(grid.GetPosCenter(cell.position), Quaternion.identity);
		// }
		SpawnTestUnitAt(Vector2Int.zero, CharacterClass.Soldier);
		SpawnTestUnitAt(new Vector2Int(3, 3), CharacterClass.Scout);
		SpawnTestUnitAt(new Vector2Int(1, 2), CharacterClass.Healer);

		SpawnTestZealotAt(new Vector2Int(7, 7));

		yield return null;
		PlayerTurn();
	}
	IEnumerator _PlayerTurn() {
		mode = MODE.PLAYER_TURN;

		List<PlayableUnit> unitSelected = new List<PlayableUnit>();

		int total_units = 0;
		foreach (Unit unit in grid.GetAllUnits()) total_units += (unit is PlayableUnit && !unit.status.AffectedBy((int) StatusEffectID.STUN)) ? 1 : 0;

		while (unitSelected.Count < total_units) {
			bool selected = false, selected_move = false, action_taken = false;
			_selected_unit = null;
			Vector2Int move_to = Vector2Int.zero;

			Action<Unit> selectUnit =  (unit) => {
				if (selected && _selected_unit == unit) return;
				WipeIndicators();
				_selected_unit = unit; selected = true;
				App.LocalInstance._UIManager.Portrait.Register(unit as PlayableUnit);
				foreach (Vector2Int nxt in _selected_unit.GetReachablePositions()) {
					GameObject indicator = moveIndicator.Borrow(grid.GetPosCenter(nxt), Quaternion.identity);
					_indicators.Add(indicator);
					indicator.GetComponent<Selectable>()?.MakeSelectable(() => {
						selected_move = true;
						move_to = nxt;
					});
				}
			};

			// all punits becomes selectable
			foreach (Unit unit in grid.GetAllUnits()) {
				if ((unit is PlayableUnit) && !unit.status.AffectedBy(((int) StatusEffectID.STUN)) && !unitSelected.Contains(unit as PlayableUnit)) {
					unit.MakeSelectable(() => selectUnit(unit));
				}
			}
			foreach (var kvp in App.LocalInstance._UIManager.SidebarManager.sidebars) {
				PlayableUnit unit = kvp.Key;
				CharacterSidebar sbar = kvp.Value;
				if (!unitSelected.Contains(unit as PlayableUnit))
					sbar.Enable(() => selectUnit(unit));
			}

			while (!selected) yield return null;
			while (!selected_move) yield return null;

			App.LocalInstance._Narrator.OnMove(_selected_unit as PlayableUnit,
				grid.GetCell(_selected_unit.position), 
				grid.GetCell(move_to));

			WipeIndicators();
			foreach (Unit unit in grid.GetAllUnits())
				if (unit is PlayableUnit && !unit.status.AffectedBy(((int) StatusEffectID.STUN))) 
					unit.DisableSelectable();

			foreach (var kvp in App.LocalInstance._UIManager.SidebarManager.sidebars) {
				PlayableUnit unit = kvp.Key;
				CharacterSidebar sbar = kvp.Value;
				sbar.Disable();
			}

			unitSelected.Add(_selected_unit as PlayableUnit);
			grid.MoveOccupant(_selected_unit.position, move_to);

			// now we get to choose an action
			UnitAction actionChosen = null;
			Cell action_cell_selected = null;

			App.LocalInstance._UIManager.AbilitySelector.StartSelector(_selected_unit as PlayableUnit, (uaction) => {
				actionChosen = uaction;
				List<Cell> targets = actionChosen.GetPossibleTargets(_selected_unit as PlayableUnit);
				WipeIndicators();
				foreach (Cell cell in targets) {
					GameObject indicator = actionIndicator.Borrow(grid.GetPosCenter(cell.position), Quaternion.identity);
					_indicators.Add(indicator);
					indicator.GetComponent<Selectable>()?.MakeSelectable(() => {
						action_taken = true;
						action_cell_selected = cell;
						actionChosen.PerformAction(_selected_unit as PlayableUnit, cell);
					});
				}
			});
			App.LocalInstance._UIManager.SkipButton.Enable("Skip Action",
			() => {
				action_taken = true;
			} );

			while (!action_taken) yield return null;
			_selected_unit.status.UpdateStatusEffects();

			App.LocalInstance._UIManager.SkipButton.Disable();
			App.LocalInstance._UIManager.AbilitySelector.StopSelectors();
			WipeIndicators();
			App.LocalInstance._UIManager.Portrait.Disable();

			yield return new WaitForSeconds(.2f);
		}
		yield return new WaitForSeconds(.3f);

		
		OtherTurn();
	}
	IEnumerator _OtherTurn() {
		mode = MODE.OTHER_TURN;

		foreach (Unit unit in grid.GetAllUnits()) {
			if (unit is AIUnit) {
				if (!unit.status.AffectedBy((int) StatusEffectID.STUN))
					grid.MoveOccupant(unit.position, (unit as AIUnit).DecideMove(grid));
				unit.status.UpdateStatusEffects();
			}
				
		}

		yield return null;

		yield return new WaitForSeconds(.5f);

		PlayerTurn();
	}
}