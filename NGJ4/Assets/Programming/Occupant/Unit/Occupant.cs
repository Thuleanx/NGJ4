using UnityEngine;

public class Occupant {
	public Grid grid;

	public Vector2Int position;
	protected GameObject gameObject;

	public Occupant(Vector2Int position) {
		this.position = position;
	}

	public void Attach(GameObject gameObject) => this.gameObject = gameObject;

	public Vector2 WorldPosition => gameObject.transform.position; 
	public virtual void Move(Vector2Int target) {
		position = target;
		if (gameObject != null) gameObject.transform.position = grid.GetPosCenter(target);
	}
}