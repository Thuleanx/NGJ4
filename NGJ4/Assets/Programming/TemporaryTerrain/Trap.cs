using UnityEngine;

public class Trap : TemporaryTerrain {
	TrapComponent trap;

	public override void Attach(GameObject gameObject) {
		base.Attach(gameObject);
	}

	public override void Enter(Occupant occupant) {
		trap.Deploy();
	}

	public override void Exit(Occupant occupant) {
	}
}