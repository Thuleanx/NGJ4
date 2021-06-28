using UnityEngine;
using System;
using System.Collections.Generic;

public class AbilitySelector : MonoBehaviour {
	AbilityToggle[] Selectors;

	void Awake() {
		Selectors = GetComponentsInChildren<AbilityToggle>(true);
	}

	void Start() {
		StopSelectors();
	}

	public void StartSelector(PlayableUnit punit, Action<UnitAction> onChooseAction) {
		List<UnitAction> actions = punit.GetActions();

		for (int i = 0; i < actions.Count; i++) {
			int x = i;
			AbilityToggle toggle = Selectors[i];

			toggle.gameObject.SetActive(true);
			toggle.Initialize(actions[i]);
			toggle.SetListener((on)=>{
				if (on) {
					onChooseAction?.Invoke(actions[x]);
				}
			});
		}
	}

	public void StopSelectors() {
		foreach (AbilityToggle toggle in Selectors) {
			toggle.ResetListeners();
			toggle.TurnOff();
			toggle.gameObject.SetActive(false);
		}
	}
}