using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ActionSelector : MonoBehaviour {
	ActionToggle[] Selectors;
	[SerializeField] ActionToggle SkipTurn;

	void Awake() {
		ActionToggle[] allToggles = GetComponentsInChildren<ActionToggle>(true);
		Selectors = new ActionToggle[allToggles.Length-1];
		bool found = false;
		for (int i = 0; i < allToggles.Length; i++) {
			if (allToggles[i] == SkipTurn) found = true;
			else Selectors[i-(found?1:0)] = allToggles[i];
		}
	}

	void Start() {
		StopSelectors();
	}

	public void StartSelector(PlayableUnit punit, Action<UnitAction> onChooseAction, Action onTurnSkip) {
		List<UnitAction> actions = punit.GetActions();

		for (int i = 0; i < actions.Count; i++) {
			int x = i;
			ActionToggle toggle = Selectors[i];

			toggle.gameObject.SetActive(true);
			toggle.Initialize(actions[i]);
			toggle.SetListener((on)=>{
				if (on) {
					onChooseAction?.Invoke(actions[x]);
				}
			});
		}
		SkipTurn.gameObject.SetActive(true);
		SkipTurn.SetListener((on) => {
			onTurnSkip?.Invoke();
		});
	}

	public void StopSelectors() {
		foreach (ActionToggle toggle in Selectors) {
			toggle.ResetListeners();
			toggle.TurnOff();
			toggle.gameObject.SetActive(false);
		}
		SkipTurn.gameObject.SetActive(false);
	}


}