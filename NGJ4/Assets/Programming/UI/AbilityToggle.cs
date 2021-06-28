using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Toggle))]
public class AbilityToggle : MonoBehaviour {
	[HideInInspector] Toggle toggle;
	[SerializeField] Image skillIcons;

	private void Awake() {
		toggle = GetComponent<Toggle>();
	}

	public void Initialize(UnitAction unitAction) {
		skillIcons.sprite = unitAction.icon;
	}

	public void SetListener(Action<bool> onValueChange) {
		toggle.onValueChanged.AddListener((on) => {onValueChange(on);});
	}

	public void TurnOff() => toggle.isOn = false;
	public void ResetListeners() => toggle.onValueChanged.RemoveAllListeners();

}