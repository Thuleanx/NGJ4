using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Unit : Occupant {
	public int range = 3;

	public int[] dx = {-1, 1, 0, 0};
	public int[] dy = {0, 0, 1, -1};

	public Unit(Vector2Int position) : base(position) {}

	public List<Vector2Int> GetReachablePositions(Grid grid) {

		List<Vector2Int> reachablePos = new List<Vector2Int>();

		Queue<Vector2Int> pq = new Queue<Vector2Int>();
		pq.Enqueue(position);

		Dictionary<Vector2Int, int> dist = new Dictionary<Vector2Int, int>();
		dist[position] = 0;

		while (pq.Count > 0) {
			Vector2Int pos = pq.Dequeue();
			if (dist[pos] + 1 <= range) for (int k = 0; k < dx.Length; k++) {
				int nx = pos.x + dx[k], ny = pos.y + dy[k];
				Vector2Int nxt = new Vector2Int(nx, ny);
				if (grid.CanMoveTo(nxt) && !dist.ContainsKey(nxt)) {
					dist[nxt] = dist[pos] + 1;
					reachablePos.Add(nxt);
					pq.Enqueue(nxt);
				}
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