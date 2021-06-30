using UnityEngine;

public class Wavy : MonoBehaviour {
	public float Amount = 2f;
	public float Period = 1.333f;
	[Range(0f, 1f)]
	public float Offset = 1f;
	float timeStart;
	Vector2 original;

	RectTransform rectTransform;

	private void Awake() {
		rectTransform = GetComponent<RectTransform>();
	}

	private void OnEnable() {
		timeStart = Time.time;
		original = rectTransform == null ? (Vector2)transform.position : rectTransform.anchoredPosition;
		Offset = Random.Range(0,1f);
	}

	void Update() {
		if (rectTransform != null)
			rectTransform.anchoredPosition = original + Wave(Time.time - timeStart);
		else
			transform.position = original + Wave(Time.time - timeStart);
	}

	public Vector2 Wave(float time) {
		return new Vector2(0f, Mathf.Sin(2 * Mathf.PI * (Offset + time / Period)) * Amount);
	}
}