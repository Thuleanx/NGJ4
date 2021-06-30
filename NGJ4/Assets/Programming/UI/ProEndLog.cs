using UnityEngine;
using TMPro;

public class ProEndLog : MonoBehaviour {
	TMP_Text Text;

	void Awake() {
		Text = GetComponent<TMP_Text>();
	}

	public void SetText(string text) =>
		Text.text = text;

	public void Clear() => Text.text = "";
}