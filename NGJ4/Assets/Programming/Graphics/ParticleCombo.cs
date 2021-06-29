using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Yarn.Unity;

public class ParticleCombo : MonoBehaviour {
	List<ParticleSystem> burstSystems = new List<ParticleSystem>();
	public bool Active { get; private set; }

	void Awake() {
		RegisterSystem();
	}

	public void RegisterSystem() {
		foreach (Transform child in transform) {
			ParticleSystem system = child.GetComponent<ParticleSystem>();
			if (system != null) burstSystems.Add(system);
		}
	}

	public void Activate() {
		if (burstSystems == null) RegisterSystem();

		foreach (ParticleSystem system in burstSystems) {
			system.Stop();
			system.Clear();
			system.Play();
		}
		Active = true;
	}

	public void Stop() {
		if (burstSystems == null) RegisterSystem();

		foreach (ParticleSystem system in burstSystems)
			system.Stop();
		Active = false;
	}
}
