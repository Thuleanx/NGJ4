using UnityEngine;

public class StatusEffect {
	public int ID;
	public int TurnsLeft;

	public StatusEffect(int ID, int Turns) {
		this.ID = ID;
		this.TurnsLeft = Turns;
	}

	// both has to be the same kind
	public StatusEffect Combine(StatusEffect other) {
		this.TurnsLeft = Mathf.Max(other.TurnsLeft, TurnsLeft);
		return this;
	}
}