using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Thuleanx.Optimization;

enum MODE {
	BEGIN,
	PLAYER_TURN,
	ENEMY_TURN,
	NATURE_TURN,
	END
}

public class GameMaster : MonoBehaviour {
	MODE mode = MODE.BEGIN;
	Grid grid;

	public int board_width = 8, board_height = 8;

	public BubblePool pawn;
	public BubblePool moveIndicator;
	public BubblePool boardBase;
	public BubblePool boardBaseAlternative;

	void Start() {
		StartGame();
	}

	public void StartGame() => StartCoroutine(_StartGame());
	public void PlayerTurn() => StartCoroutine(_PlayerTurn());

	public void SpawnTestUnitAt(Vector2Int position) {
		Unit unit = new Unit(position);
		GameObject pawnObj = pawn.Borrow(grid.GetPosCenter(unit.position), Quaternion.identity);
		unit.range = 4;
		unit.Attach(pawnObj);
		grid.AddOccupant(unit.position, unit);
	}

	IEnumerator _StartGame() {
		mode = MODE.BEGIN;

		// construct the grid logically
		grid = new Grid(8, 8, transform.position, 1f);
		// instantiating the grid 
		foreach (Cell cell in grid.arr) {
			if ((cell.x + cell.y)%2 == 0)	boardBase.Borrow(grid.GetPosCenter(cell.position), Quaternion.identity);
			else 							boardBaseAlternative.Borrow(grid.GetPosCenter(cell.position), Quaternion.identity);
		}
		SpawnTestUnitAt(Vector2Int.zero);
		SpawnTestUnitAt(new Vector2Int(3, 3));

		yield return null;
		PlayerTurn();
	}
	IEnumerator _PlayerTurn() {
		mode = MODE.PLAYER_TURN;

		Debug.Log("START PLAYER TURN");

		bool selected = false;
		Unit selected_unit = null;
		bool selected_move = false;
		Vector2Int move_to = Vector2Int.zero;
		List<GameObject> indicators = new List<GameObject>();

		// all units becomes selectable
		foreach (Unit unit in grid.GetAllUnits()) {
			unit.MakeSelectable(() => {
				if (selected && selected_unit == unit) return ;

				if (selected)
					foreach (GameObject obj in indicators)
						obj.SetActive(false);

				indicators = new List<GameObject>();

				selected_unit = unit;
				selected = true;

				Debug.Log(unit.GetReachablePositions(grid).Count);

				foreach (Vector2Int nxt in selected_unit.GetReachablePositions(grid)) {
					GameObject indicator = moveIndicator.Borrow(grid.GetPosCenter(nxt), Quaternion.identity);
					indicators.Add(indicator);
					indicator.GetComponent<Selectable>()?.MakeSelectable(() => {
						selected_move = true;
						move_to = nxt;
					});
				}
			});
		}

		while (!selected) yield return null;

		Debug.Log("UNIT SELECTED");

		// do something

		while (!selected_move) yield return null;

		grid.MoveOccupant(selected_unit.position, move_to);


		foreach (GameObject obj in indicators)
			obj.SetActive(false);

		// all units becomes unselectable
		foreach (Unit unit in grid.GetAllUnits())
			unit.DisableSelectable();
		
		PlayerTurn();
	}
}