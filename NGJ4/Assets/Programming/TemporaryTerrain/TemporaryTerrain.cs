
using UnityEngine;

public class TemporaryTerrain {
	public Grid grid;

	public Vector2Int position;
	protected GameObject gameObject;

	public virtual void Attach(GameObject gameObject) => this.gameObject = gameObject;
	public virtual void Enter(Occupant occupant) { }
	public virtual void Exit(Occupant occupant) { }
}