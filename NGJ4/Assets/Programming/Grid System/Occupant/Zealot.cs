using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Zealot : AIUnit {
	public Zealot(Vector2Int pos) : base(pos) {}

	public override Vector2Int DecideMove(Grid grid) {

		Pawn pawn = null;
		foreach (Unit unit in grid.GetAllUnits()) {
			if (unit is Pawn && 
				(pawn == null || Grid.Approx_Distance(position, unit.position) < 
				Grid.Approx_Distance(position, pawn.position))) {

				pawn = unit as Pawn;
			}
		}

		Path path = grid.GetPathBetween(position, pawn.position);
		path.pop_node();
		string res = "";
		foreach (Vector2Int point in path.nodes) {
			res += point.ToString() + " ";
		}
		Debug.Log(res);
		return range < path.Length ? path.nodes[range] : path.Last;
	}
}