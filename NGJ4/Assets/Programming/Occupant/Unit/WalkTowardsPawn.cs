using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Thuleanx.Optimization;
using Thuleanx.Math;

public class WalkTowardsPawns : AIUnit {
	public int range = 3;
	public int attack_range = 3;
	public int attack_damage = 1;
	public BubblePool effect;

	public WalkTowardsPawns(Vector2Int pos) : base(pos) {}

	public override Vector2Int DecideMove(Grid grid) {
		PlayableUnit pawn = null;
		foreach (Unit unit in grid.GetAllUnits()) if (!unit.IsDead()) {
			if (unit is PlayableUnit && 
				(pawn == null || Grid.Approx_Distance(position, unit.position) < 
				Grid.Approx_Distance(position, pawn.position)) &&
				Grid.Approx_Distance(position, unit.position) <= vision) {

				pawn = unit as PlayableUnit;
			}
		}

		if (pawn != null) {
			Path path = grid.GetPathBetween(position, pawn.position);
			if (path.Length > 0) {
				path.pop_node();
				string res = "";
				foreach (Vector2Int point in path.nodes) {
					res += point.ToString() + " ";
				}
				return range < path.Length ? path.nodes[range] : path.Last;
			} else return position;
		}
		return position;
	}

	public override bool DecideAction() {
		PlayableUnit pawn = null;
		foreach (Unit unit in grid.GetAllUnits()) if (!unit.IsDead())
		if (unit.position.x == position.x || unit.position.y == position.y) {
			if (unit is PlayableUnit && 
				(pawn == null || 
				Grid.Approx_Distance(position, unit.position) < Grid.Approx_Distance(position, pawn.position))) {

				// in sight and in range
				pawn = unit as PlayableUnit;
			}
		}
		if (pawn != null && Grid.Approx_Distance(position, pawn.position) 
			<= attack_range) {
			effect?.Borrow(grid.GetPosCenter(pawn.position), 
				Calc.ToQuat(pawn.position - position));
			pawn.TakeDamage(this, attack_damage);
			gameObject.GetComponent<UnitObject>().OnAttack();
			return true;
		}
		return false;
	}
}