using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterSidebar : MonoBehaviour {
	[SerializeField] Button FocusButton;
	[SerializeField] Image CharacterSprite;
	PlayableUnit punit;

	public void AttachUnit(PlayableUnit unit) {
		punit = unit;
		CharacterSprite.sprite = unit.characterSprite;
	}

	void Start() {
		Disable();
	}

	public void Disable() {
		FocusButton.interactable = false;
		FocusButton.onClick.RemoveAllListeners();
	}

	public void Enable(Action action) {
		FocusButton.interactable = true;
		FocusButton.onClick.AddListener(() => {action?.Invoke(); });
	}
}