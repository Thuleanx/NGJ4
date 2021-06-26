using UnityEngine;
using System.Collections.Generic;

public class Reposition : UnitAction {
	public int range = 1;

	public Reposition(int id, string name) : base(id, name) {}

	public override List<Cell> GetPossibleTargets(PlayableUnit punit) {
		List<Cell> cells = new List<Cell>();

		int[] dx = {1, -1, 0, 0};
		int[] dy = {0, 0, 1, -1};

		for (int k = 0; k < dx.Length; k++) {
			for (int z = 1; z <= range; z++) {
				Vector2Int target = punit.position + 
					(z * new Vector2Int(dx[k], dy[k]));
				if (!punit.grid.InGrid(target)) break;
				Cell other = punit.grid.GetCell(target.x, target.y);
				if (other.hasOccupant && (other.Occupant is PlayableUnit)) {
					cells.Add(other);
				} else break;
			}
		}

		return cells;
	}

	public override void PerformAction(PlayableUnit punit, Cell other) {
		punit.grid.SwapOccupant(punit.position, other.position);
	}
}