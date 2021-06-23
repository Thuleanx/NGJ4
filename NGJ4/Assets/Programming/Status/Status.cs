using UnityEngine;
using System.Collections.Generic;

public struct Status {
	int health;
	public int Health {
		get { return health; }
		set { 
			health = value;
			health = Mathf.Clamp(health,0,MaxHealth);
		}
	}
	public int MaxHealth;
	Dictionary<int, StatusEffect> effectActive;

	public Status(int MaxHealth) {
		this.MaxHealth = MaxHealth;
		health = MaxHealth;
		effectActive = new Dictionary<int, StatusEffect>();
	}

	public void UpdateStatusEffects() {
		foreach (int id in (new List<int>(effectActive.Keys))) {
			if (--effectActive[id].TurnsLeft == 0)
				effectActive.Remove(id);
		}
	}
	public void Apply(StatusEffect effect) {
		if (effectActive.ContainsKey(effect.ID))
			effectActive[effect.ID] = effect.Combine(effectActive[effect.ID]);
		else 
			effectActive[effect.ID] = effect;
	}
	public bool AffectedBy(int id) => effectActive.ContainsKey(id);
}