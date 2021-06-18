using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Unit : Occupant {
	public int range = 4;

	public Unit(Vector2Int position) : base(position) {}

	public List<Vector2Int> GetReachablePositions(Grid grid) {

		List<Vector2Int> reachablePos = new List<Vector2Int>();

		for (int dx = -range; dx <= range; dx++) {
			int left = (range - Mathf.Abs(dx));
			for (int dy = -left; dy <= left; dy++) {
				if ((dx != 0 || dy != 0) && grid.CanMoveTo(position + (new Vector2Int(dx, dy))))
					reachablePos.Add(position + (new Vector2Int(dx,dy)));
			}
		}
		
		return reachablePos;
	}

	public void MakeSelectable(Action onSelect) {
		gameObject.GetComponent<Selectable>().MakeSelectable(onSelect);
	}

	public void DisableSelectable() {
		gameObject.GetComponent<Selectable>().DisableSelectable();
	}
}