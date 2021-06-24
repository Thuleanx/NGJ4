using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Zealot : AIUnit {
	public int range = 3;

	public Zealot(Vector2Int pos) : base(pos) {}

	public override Vector2Int DecideMove(Grid grid) {
		PlayableUnit pawn = null;
		foreach (Unit unit in grid.GetAllUnits()) {
			if (unit is PlayableUnit && 
				(pawn == null || Grid.Approx_Distance(position, unit.position) < 
				Grid.Approx_Distance(position, pawn.position))) {

				pawn = unit as PlayableUnit;
			}
		}

		Path path = grid.GetPathBetween(position, pawn.position);
		path.pop_node();
		string res = "";
		foreach (Vector2Int point in path.nodes) {
			res += point.ToString() + " ";
		}
		return range < path.Length ? path.nodes[range] : path.Last;
	}
}