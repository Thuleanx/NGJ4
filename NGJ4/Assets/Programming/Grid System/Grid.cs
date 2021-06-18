using UnityEngine;
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

	public Cell GetCellContent(int x, int y) => arr[x,y];
	public Vector2Int GetCell(Vector2 worldPosition) => Vector2Int.FloorToInt(worldPosition);
	public Vector2 GetPosCenter(Vector2Int boardPosition) => boardPosition * Cell2DSize + origin + Cell2DSize/2;

	public int MoveOccupant(Vector2Int start, Vector2Int end) {
		if (!arr[start.x, start.y].hasOccupant) return -1;
		if (arr[end.x,end.y].hasOccupant) return -1;

		Occupant occupant = arr[start.x, start.y].occupant;
		occupant.Move(this, end);
		arr[start.x,start.y].occupant = null;
		arr[end.x,end.y].occupant = occupant;

		return 0;
	}

	public int AddOccupant(Vector2Int pos, Occupant occupant) {
		if (arr[pos.x, pos.y].hasOccupant)
			return -1;
		arr[pos.x, pos.y].occupant = occupant;
		occupants.Add(occupant);
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

	public bool CanMoveTo(Vector2Int pos) {
		return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height && !GetCellContent(pos.x, pos.y).hasOccupant;
	}
}