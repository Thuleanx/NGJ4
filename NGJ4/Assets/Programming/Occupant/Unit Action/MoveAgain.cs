using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MoveAgain", menuName = "~/Ability/MoveAgain", order = 0)]
public class MoveAgain : UnitAction {
	public MoveAgain(string name) : base(name) {}

	public override List<Cell> GetPossibleTargets(PlayableUnit punit) {
		List<Cell> cells = new List<Cell>();
		foreach (Vector2Int pos in punit.GetReachablePositions())
			cells.Add(punit.grid.GetCell(pos));
		return cells;
	}

	public override void PerformAction(PlayableUnit punit, Cell other) {
		punit.grid.MoveOccupant(punit.position, other.position);
	}
}