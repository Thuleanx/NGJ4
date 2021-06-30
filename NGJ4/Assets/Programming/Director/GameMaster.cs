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
	public BubblePool moveIndicator, actionIndicator, rockChunk;
	public UnitMold Soldier, Healer, Scout, 
		Zealot, Scavenger, Corrupted;

	public int TimeLimit = 28;

	Dictionary<PlayableUnit, HashSet<int>> AreaVisited =
		new Dictionary<PlayableUnit, HashSet<int>>();

	public int RocksGathered;
	public int GameTime;

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
		AreaVisited[punit] = new HashSet<int>();
	}
	public void SpawnTestZealotAt(Vector2Int position) {
		AIUnit aunit = Zealot.Create_E(position, grid.GetPosCenter(position));
		grid.AddOccupant(position, aunit);
	}

	#region Player Turn Variables
	List<GameObject> _indicators = new List<GameObject>();
	Unit _selected_unit = null;
	#endregion

	public void WipeIndicators() {
		foreach (GameObject obj in _indicators) obj.SetActive(false);
		_indicators.Clear();
	}

	public void LoadBiomes() {
		foreach (BiomeLabel biomeLabel in FindObjectsOfType<BiomeLabel>()) {
			Vector2Int position = grid.WorldToGridPosition(
				biomeLabel.transform.position);
			Cell cell = grid.GetCell(position);
			cell.biome = biomeLabel.biome;
		}
		grid.FloodFill();
	}
	public void LoadObstacles() {
		foreach (ObstacleObject obstacle in FindObjectsOfType<ObstacleObject>()) {
			Vector2Int position = grid.WorldToGridPosition(obstacle.transform.position);
			grid.AddOccupant(
				position,obstacle.Create(position));
		}
	}
	public void PopulateRocks() {
		mt19937 rng = new mt19937();
		for (int x = 0; x < board_width; x++) {
			for (int y = 0; y < board_height; y++) {
				if (!grid.GetCell(x,y).hasOccupant &&
					rng.Next() < 1 / grid.GetBiome(x,y).biomeType.MeteoriteSpawnRateInverse()) {
					Rock chunk = new Rock();
					chunk.Attach(rockChunk.Borrow(grid.GetPosCenter(new Vector2Int(x,y)), 
						Quaternion.identity));
					grid.AddTerrain(chunk, new Vector2Int(x,y));
				}
			}
		}
	}
	public void SpawnEnemies() {
		mt19937 rng = new mt19937();
		for (int x = 0; x < board_width; x++) {
			for (int y = 0; y < board_height; y++) {
				Cell cell = grid.GetCell(x,y);
				Vector2Int position = new Vector2Int(x, y);
				if (!cell.hasOccupant && cell.Terrains.Count == 0 &&
					rng.Next() < 1 / grid.GetBiome(x,y).biomeType.SpawnRateInverse()) {

					AIUnit aunit = null;
					switch (cell.biome.biomeType.EnemySpawn()) {
						case EnemyClass.ZEALOT: 
							aunit = Zealot.Create_E(position, grid.GetPosCenter(position));
							break;
						case EnemyClass.CORRUPTED: 
							aunit = Corrupted.Create_E(position, grid.GetPosCenter(position));
							break;
						case EnemyClass.SCAVENGER: 
							aunit = Scavenger.Create_E(position, grid.GetPosCenter(position));
							break;
					}
					if (aunit != null) grid.AddOccupant(position, aunit);
				}
			}
		}
	}

	IEnumerator _StartGame() {
		mode = MODE.BEGIN;
		yield return new WaitForEndOfFrame();

		App.LocalInstance._UIManager.SetInvisible();

		// construct the grid logically
		grid = new Grid(board_width, board_height, transform.position, 1f);

		RocksGathered = 0;
		GameTime = 0;
		LoadBiomes();
		LoadObstacles();
		PopulateRocks();
		SpawnEnemies();

		SpawnTestUnitAt(Vector2Int.zero, CharacterClass.Soldier);
		SpawnTestUnitAt(new Vector2Int(3, 3), CharacterClass.Scout);
		SpawnTestUnitAt(new Vector2Int(1, 2), CharacterClass.Healer);

		SpawnTestZealotAt(new Vector2Int(6, 6));

		App.LocalInstance._UIManager.SetVisible();

		yield return null;
		PlayerTurn();
	}
	IEnumerator _PlayerTurn() {
		mode = MODE.PLAYER_TURN;

		List<PlayableUnit> unitSelected = new List<PlayableUnit>();

		int total_units = 0;
		foreach (Unit unit in grid.GetAllUnits()) total_units += (unit is PlayableUnit && unit.CanMove()) ? 1 : 0;

		while (unitSelected.Count < total_units) {

			App.LocalInstance._UIManager.SkipButton.SetText("Your Turn");

			bool selected = false, selected_move = false, action_taken = false;
			_selected_unit = null;
			Vector2Int move_to = Vector2Int.zero;

			Action<Unit> selectUnit =  (unit) => {
				App.LocalInstance._Camera.FollowUnit(unit);
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
				if ((unit is PlayableUnit) && unit.CanMove() && !unitSelected.Contains(unit as PlayableUnit)) {
					unit.MakeSelectable(() => selectUnit(unit));
				}
			}
			foreach (var kvp in App.LocalInstance._UIManager.SidebarManager.sidebars) {
				PlayableUnit unit = kvp.Key;
				if (unit.CanMove()) {
					CharacterSidebar sbar = kvp.Value;
					if (!unitSelected.Contains(unit as PlayableUnit))
						sbar.Enable(() => selectUnit(unit));
				}
			}

			while (!selected) yield return null;
			while (!selected_move) yield return null;

			int biomeCurrent = grid.CellGroup(_selected_unit.position);
			int biomeNxt = grid.CellGroup(move_to);

			if (biomeNxt != biomeCurrent && !AreaVisited[_selected_unit as PlayableUnit].Contains(biomeNxt)) {
				App.LocalInstance._Narrator.OnBiomeMove(_selected_unit as PlayableUnit,
					grid.GetCell(_selected_unit.position), 
					grid.GetCell(move_to));
				AreaVisited[_selected_unit as PlayableUnit].Add(biomeNxt);
			}

			WipeIndicators();
			foreach (Unit unit in grid.GetAllUnits())
				if (unit is PlayableUnit && unit.CanMove()) 
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
		App.LocalInstance._UIManager.SkipButton.SetText("Enemy Turn");

		foreach (Unit unit in grid.GetAllUnits()) {
			if (unit is AIUnit) {
				if (unit.CanMove()) {
					App.LocalInstance._Camera.FollowUnit(unit);
					grid.MoveOccupant(unit.position, (unit as AIUnit).DecideMove(grid));
					if ((unit as AIUnit).DecideAction()) {
						yield return new WaitForSeconds(.5f);
					}
				}
				unit.status.UpdateStatusEffects();
			}
				
		}
		yield return null;

		yield return new WaitForSeconds(.5f);
		if (++GameTime == TimeLimit) {
		}

		PlayerTurn();
	}
}