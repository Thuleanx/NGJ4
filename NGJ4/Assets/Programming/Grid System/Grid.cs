using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct Grid {
	int width, height;
	float cellSize;
	Vector2 origin;
	public Cell[,] arr;
	List<Occupant> occupants;

	public Vector2 Cell2DSize => new Vector2(cellSize, cellSize);

	public Grid(int width, int height, Vector2 origin, float cellSize) {
		this.width = width;
		this.height = height;
		this.origin = origin;
		this.cellSize = cellSize;
		occupants = new List<Occupant>();

		arr = new Cell[width,height];
		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				arr[x,y] = new Cell(x,y);
	}

	public Cell GetCell(int x, int y) => arr[x,y];
	public Cell GetCell(Vector2Int pos) => arr[pos.x, pos.y];
	public Vector2Int WorldToGridPosition(Vector2 worldPosition) => Vector2Int.FloorToInt(worldPosition);
	public Vector2 GetPosCenter(Vector2Int boardPosition) => boardPosition * Cell2DSize + origin + Cell2DSize/2;

	public static int Approx_Distance(Vector2Int a, Vector2Int b) => Mathf.Abs((a-b).x) + Mathf.Abs((a-b).y);

	public int MoveOccupant(Vector2Int start, Vector2Int end) {
		if (!arr[start.x, start.y].hasOccupant) return -1;
		if (arr[end.x,end.y].hasOccupant) return -1;

		Occupant Occupant = arr[start.x, start.y].Occupant;
		Occupant.Move(end);

		arr[start.x,start.y].RemoveOccupant();
		arr[end.x,end.y].SetOccupant(Occupant);

		return 0;
	}

	public int SwapOccupant(Vector2Int a, Vector2Int b) {
		if (!arr[a.x,a.y].hasOccupant) return -1;
		if (!arr[b.x,b.y].hasOccupant) return -1;


		Occupant Occupant = arr[a.x,a.y].Occupant;
		Occupant Occupant2 = arr[b.x,b.y].Occupant;

		Occupant.Move(b);
		Occupant2.Move(a);
		
		arr[a.x, a.y].RemoveOccupant();
		arr[b.x, b.y].RemoveOccupant();

		arr[a.x, a.y].SetOccupant(Occupant2);
		arr[b.x, b.y].SetOccupant(Occupant);

		return 0;
	}

	public int AddOccupant(Vector2Int pos, Occupant Occupant) {
		if (arr[pos.x, pos.y].hasOccupant)
			return -1;
		arr[pos.x, pos.y].SetOccupant(Occupant);
		Occupant.grid = this;
		occupants.Add(Occupant);
		return 0;
	}

	public List<Occupant> GetAllOccupants() => occupants;
	public List<Unit> GetAllUnits() {
		List<Unit> units = new List<Unit>();
		foreach (Occupant occupant in occupants) {
			if (occupant is Unit) units.Add(occupant as Unit);
		}
		return units;
	}

	public bool InGrid(Vector2Int pos) {
		return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
	}

	public bool Unoccupied(Vector2Int pos) {
		return InGrid(pos) && !GetCell(pos.x, pos.y).hasOccupant;
	}
	public Path GetPathBetween(Vector2Int start, Vector2Int end, int[] dx = null, int[] dy = null) {
		dx = dx ?? new int[]{1, -1, 0, 0};
		dy = dy ?? new int[]{0, 0, -1, 1};

		if (start == end) {
			Path ans = new Path();
			ans.add_node(end);
			return ans;
		}

		PriorityQueue<Vector2Int, int> pq = new PriorityQueue<Vector2Int, int>();
		Dictionary<Vector2Int, int> g = new Dictionary<Vector2Int, int>();
		Dictionary<Vector2Int, Vector2Int> prev = new Dictionary<Vector2Int, Vector2Int>();
		Func<Vector2Int, int> h = (pos) => {
			return Approx_Distance(pos, end);
		};

		Func<Vector2Int, int> f = (pos) => {
			if (!g.ContainsKey(pos)) return int.MaxValue;
			return g[pos] + h(pos);
		};
		g[start] = 0;
		pq.push(start, f(start));


		while (f(end) == int.MaxValue && pq.Count > 0) {
			Vector2Int cur = pq.peek(); 
			int prio = pq.peek_prio();
			pq.pop();
			if (prio != f(cur)) continue;

			for (int k = 0; k < dx.Length; k++) {
				int nx = cur.x + dx[k], ny = cur.y + dy[k];
				Vector2Int nxt = new Vector2Int(nx, ny);

				if (Unoccupied(nxt) || nxt == end) {
					int dist = g[cur] + 1;

					if (!g.ContainsKey(nxt) || dist < g[nxt]) {
						g[nxt] = dist;
						prev[nxt] = cur;
						pq.push(nxt, f(nxt));
					}
				}
			}
		}

		if (f(end) == int.MaxValue) return null;

		Path res = new Path();
		while (end != start) {
			res.add_node(end);
			end = prev[end];
		}
		res.add_node(start);
		res.reverse();

		return res;
	}
}