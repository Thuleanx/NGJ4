using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

[RequireComponent(typeof(Button))]
public class SkipButton : MonoBehaviour {
	Button button;
	TMP_Text text;

	void Awake() {
		button = GetComponent<Button>();
		text = GetComponentInChildren<TMP_Text>();
	}

	void Start() => Disable();

	public void Disable() {
		button.interactable = false;
		text.text = "WAITING";
		button.onClick.RemoveAllListeners();
	}

	public void Enable(string buttonText, Action action) {
		button.interactable = true;
		text.text = buttonText;
		button.onClick.AddListener(() => {action?.Invoke();});
	}

	public void SetText(string buttonText) => text.text = buttonText;
}