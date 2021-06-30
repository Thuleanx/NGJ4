using UnityEngine;

public class AIUnit : Unit {
	public EnemyInfo info;
	public Sprite characterSprite;

	public AIUnit(Vector2Int pos) : base(pos) {}

	public virtual Vector2Int DecideMove(Grid grid) => position;

	public virtual bool DecideAction() => false;

	public override void OnDeath() {
		grid.RemoveOccupant(position);
		gameObject.GetComponent<UnitObject>().OnDeath();
	}

}