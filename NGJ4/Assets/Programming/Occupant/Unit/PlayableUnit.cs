
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayableUnit : Unit {
	public int range = 3;
	public Sprite characterSprite;
	public CharacterInfo info;
	List<UnitAction> actions = new List<UnitAction>();

	public PlayableUnit(Vector2Int pos) : base(pos) {}

	public virtual List<UnitAction> GetActions() => actions;
	public virtual void AddAction(UnitAction action) => actions.Add(action);

	public override List<Vector2Int> GetReachablePositions(Grid grid) {
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
				if (grid.Unoccupied(nxt) && !dist.ContainsKey(nxt)) {
					dist[nxt] = dist[pos] + 1;
					reachablePos.Add(nxt);
					pq.Enqueue(nxt);
				}
			}
		}

		return reachablePos;
	}
}