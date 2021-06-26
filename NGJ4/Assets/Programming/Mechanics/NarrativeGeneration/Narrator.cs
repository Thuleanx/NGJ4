using UnityEngine;
using Thuleanx;
using Thuleanx.Optimization;
using TMPro;

public class Narrator : MonoBehaviour {
	public GameObject parchment;

	[SerializeField] BubblePool textPool;

	public void OnMove(PlayableUnit punit, Cell start, Cell end) {
		App.Instance._NarrativeGenerator.ClearOverrides();
		App.Instance._NarrativeGenerator.Load(punit.info);

		AddLine(App.Instance._NarrativeGenerator.parse("> #mvb"));
	}

	void AddLine(string line) {
		GameObject textObj = textPool.Borrow();
		textObj.GetComponent<TMP_Text>().text = line;

		textObj.transform.SetParent(parchment.transform);

		RectTransform rectTransform = textObj.GetComponent<RectTransform>();
		if (rectTransform != null) {
			rectTransform.localScale = new Vector3(1, 1, 1);
			rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
		}

	}
}