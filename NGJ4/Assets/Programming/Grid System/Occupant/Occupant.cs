using UnityEngine;

public class Occupant {
	public Vector2Int position;
	protected GameObject gameObject;

	public Occupant(Vector2Int position) {
		this.position = position;
	}

	public virtual void Move(Grid grid, Vector2Int target) {
		position = target;
		gameObject.transform.position = grid.GetPosCenter(target);
	}

	public void Attach(GameObject gameObject) => this.gameObject = gameObject;
}