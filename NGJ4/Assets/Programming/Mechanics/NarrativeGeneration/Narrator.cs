using UnityEngine;
using Thuleanx;
using Thuleanx.Optimization;
using TMPro;

public class Narrator : MonoBehaviour {
	public GameObject parchment;

	[SerializeField] BubblePool textPool;
	[SerializeField, FMODUnity.EventRef] string writingEvent;
	public float AbilityFrequency = 1/15f;

	public void OnStart(PlayableUnit p1, PlayableUnit p2, PlayableUnit p3) {
		App.Instance._NarrativeGenerator.ClearOverrides();
		App.Instance._NarrativeGenerator.Load(p1, p2, p3);

		App.LocalInstance._Log.SetText(App.Instance.
			_NarrativeGenerator.parse("#prlg"));
	}

	public void OnEnd(PlayableUnit p1, PlayableUnit p2, PlayableUnit p3, int VERSION) {
		App.Instance._NarrativeGenerator.ClearOverrides();
		App.Instance._NarrativeGenerator.Load(p1, p2, p3);

		App.LocalInstance._Log.SetText(App.Instance.
			_NarrativeGenerator.parse("#eplg" + VERSION));
	}

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
		} else {
			App.Instance._NarrativeGenerator.Load((target as PlayableUnit).info);
			AddLine(App.Instance._NarrativeGenerator.parse("> #cdc"));
		}
	}

	public void OnShoulderBash(Unit person, Unit target) {
		if (Random.Range(0, 1f) < AbilityFrequency) {
			App.Instance._NarrativeGenerator.Load((person as PlayableUnit).info);
			App.Instance._NarrativeGenerator.Load((target as AIUnit).info);

			AddLine(App.Instance._NarrativeGenerator.parse("> #stu1"));
		}
	}

	public void OnReposition(Unit person, Unit target) {
		if (Random.Range(0, 1f) < AbilityFrequency)
			AddLine(App.Instance._NarrativeGenerator.parse("> #stu2"));
	}

	public void OnHeal(Unit person, Unit target) {
		if (Random.Range(0, 1f) < AbilityFrequency) {
			App.Instance._NarrativeGenerator.Load((person as PlayableUnit).info);
			AddLine(App.Instance._NarrativeGenerator.parse("> #htu2"));
		}
	}

	public void OnPray(Unit person) {
		App.Instance._NarrativeGenerator.Load((person as PlayableUnit).info);
		AddLine(App.Instance._NarrativeGenerator.parse("> #htu1"));
	}

	public void OnShot(Unit person, Unit target) {
		if (Random.Range(0, 1f) < AbilityFrequency) {
			App.Instance._NarrativeGenerator.Load((person as PlayableUnit).info);
			App.Instance._NarrativeGenerator.Load((target as AIUnit).info);
			AddLine(App.Instance._NarrativeGenerator.parse("> #rtu1"));
		}
	}

	public void OnMoveAgain(Unit person) {
		if (Random.Range(0, 1f) < AbilityFrequency) {
			App.Instance._NarrativeGenerator.Load((person as PlayableUnit).info);
			AddLine(App.Instance._NarrativeGenerator.parse("> #rtu2"));
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

		App.Instance._AudioManager.PlayOneShot(writingEvent);

	}
}