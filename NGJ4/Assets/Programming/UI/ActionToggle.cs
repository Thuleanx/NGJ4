using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;

using Thuleanx.Utility;

[RequireComponent(typeof(Toggle))]
public class ActionToggle : MonoBehaviour {
	[HideInInspector] Toggle toggle;
	public Optional<TMP_Text> title_text;

	private void Awake() {
		toggle = GetComponent<Toggle>();
		if (!title_text.Enabled) title_text = new Optional<TMP_Text>(GetComponentInChildren<TMP_Text>());
	}

	public void Initialize(UnitAction unitAction) {
		title_text.Value.text = unitAction.ActionName;
	}

	public void SetListener(Action<bool> onValueChange) {
		toggle.onValueChanged.AddListener((on) => {onValueChange(on);});
	}

	public void TurnOff() => toggle.isOn = false;
	public void ResetListeners() => toggle.onValueChanged.RemoveAllListeners();
}