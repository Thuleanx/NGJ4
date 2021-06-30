using UnityEngine;
using System;
using Thuleanx;
using TMPro;

public class DayTracker : MonoBehaviour {
	TMP_Text Text;
	int lastTime = -1;
	void Awake() {
		Text = GetComponent<TMP_Text>();
	}

	void Update() {
		int time = App.Instance._GameMaster.GameTime;
		if (time != lastTime) {
			int date = time / 4 + 1;
			TimeOfDay daytime = (TimeOfDay) (time%4);
			int daysFromExtraction = (App.Instance._GameMaster.TimeLimit - time + 3)/4;
			Text.text = String.Format("Day {0}. {1}. {2} from extraction", date, daytime.Name(), daysFromExtraction);
		}
		lastTime = time;
	}
}