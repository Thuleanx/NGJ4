using UnityEngine;
using System;
using Thuleanx;
using TMPro;

public class RockTracker : MonoBehaviour {
	TMP_Text Text;
	int lastNum = -1;
	void Awake() {
		Text = GetComponent<TMP_Text>();
	}

	void Update() {
		int num = App.Instance._GameMaster.RocksGathered;
		if (num != lastNum)
			Text.text = String.Format("{0} Lokthlokh Chunks", num);
		lastNum = num;
	}
}