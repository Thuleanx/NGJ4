using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell {
	public Vector2Int position;
	public Occupant occupant;

	public int x => position.x;
	public int y => position.y;

	public Cell(int x, int y) {
		this.position = new Vector2Int(x,y);
		occupant = null;
	}

	public bool hasOccupant => occupant != null;
}