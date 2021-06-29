using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell {
	public Vector2Int position;
	Occupant occupant;
	public List<TemporaryTerrain> Terrains = new List<TemporaryTerrain>();
	public Biome biome;

	public Occupant Occupant {
		get { return occupant; }
	}

	public int x => position.x;
	public int y => position.y;

	public Cell(int x, int y) {
		this.position = new Vector2Int(x,y);
		occupant = null;
	}
	public bool hasOccupant => occupant != null;

	public void RemoveOccupant() {
		foreach (TemporaryTerrain terrain in Terrains)
			terrain.Exit(occupant);
		occupant = null;
	}
	public void SetOccupant(Occupant occupant) {
		foreach (TemporaryTerrain terrain in Terrains)
			terrain.Enter(occupant);
		this.occupant = occupant;
	} 
}