using UnityEngine;

public class AIUnit : Unit {
	public AIUnit(Vector2Int pos) : base(pos) {}

	public virtual Vector2Int DecideMove(Grid grid) => position;
}