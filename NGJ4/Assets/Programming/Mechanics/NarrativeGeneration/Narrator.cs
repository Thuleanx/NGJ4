using UnityEngine;
using Thuleanx;
using Thuleanx.Optimization;
using TMPro;

public class Narrator : MonoBehaviour {
	public GameObject parchment;

	[SerializeField] BubblePool textPool;

	public void OnBiomeMove(PlayableUnit punit, Cell start, Cell end) {
		App.Instance._NarrativeGenerator.ClearOverrides();
		App.Instance._NarrativeGenerator.Load(punit.info);

		if (start.biome.biomeType == end.biome.biomeType && end.biome.biomeType == BiomeType.PLAIN) {
			// does nothing
		}  else if (Random.Range(0, 1f) < .2f || end.biome.biomeType == BiomeType.PLAIN) {
			// out of biome
			AddLine(App.Instance._NarrativeGenerator.parse("> " + start.biome.biomeType.MoveOut()));
		} else {
			AddLine(App.Instance._NarrativeGenerator.parse("> " + end.biome.biomeType.MoveIn()));
		}
	}

	public void OnCollectRock(PlayableUnit punit, Cell cell) {
		App.Instance._NarrativeGenerator.ClearOverrides();
		App.Instance._NarrativeGenerator.Load(punit.info);

		AddLine(App.Instance._NarrativeGenerator.parse("> " + 
			cell.biome.biomeType.MeteoriteCollect()));
	}

	public void OnKill(Unit person, Unit target, Cell cell) {
		App.Instance._NarrativeGenerator.ClearOverrides();
		if (target is AIUnit) {
			App.Instance._NarrativeGenerator.Load((person as PlayableUnit).info);
			App.Instance._NarrativeGenerator.Load((target as AIUnit).info);

			string prompt = (target as AIUnit).info.enemyType.DeathNarrative(cell.biome.biomeType);
			AddLine(App.Instance._NarrativeGenerator.parse("> " + prompt));
		}
	}

	void AddLine(string line) {
		AddLine(line, Color.white);
	}

	void AddLine(string line, Color textColor) {
		GameObject textObj = textPool.Borrow();
		textObj.GetComponent<TMP_Text>().text = line;
		textObj.GetComponent<TMP_Text>().color = textColor;

		textObj.transform.SetParent(parchment.transform);

		RectTransform rectTransform = textObj.GetComponent<RectTransform>();
		if (rectTransform != null) {
			rectTransform.localScale = new Vector3(1, 1, 1);
			rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
		}

	}
}