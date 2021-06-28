using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Snipe", menuName = "~/Ability/Snipe", order = 0)]
public class Snipe : UnitAction {
	public int range = 4;
	public int push_distance = 1;

	public Snipe(string name) : base(name) {}

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
				if (other.hasOccupant && !(other.Occupant is PlayableUnit)) {
					cells.Add(other);
				} 
			}
		}

		return cells;
	}
	public override void PerformAction(PlayableUnit punit, Cell other) {
		Vector2Int kbDir = (other.position - punit.position);
		kbDir /= Mathf.Abs(kbDir.x+kbDir.y);
		Unit uother = other.Occupant as Unit;
		uother.Knockback(kbDir * push_distance);
	}
}