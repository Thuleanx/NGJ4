using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Thuleanx;

[RequireComponent(typeof(Button))]
public class ContinueButton : MonoBehaviour {
	Button button;

	void Awake() {
		button = GetComponent<Button>();
	}

	public void Disable() {
		gameObject.SetActive(false);
		button.onClick.RemoveAllListeners();
	}

	public void Enable(string prompt) {
		GetComponentInChildren<TMP_Text>().text = prompt;
		gameObject.SetActive(true);
		button.onClick.AddListener(() => {
			App.Instance._GameMaster.Continue();
		});
	}
}