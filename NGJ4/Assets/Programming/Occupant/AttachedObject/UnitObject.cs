using UnityEngine;
using TMPro;

public class UnitObject : MonoBehaviour {
	Unit unit;
	public TMP_Text health_text;

	public void AttachUnit(Unit unit) {
		this.unit = unit;
	}

	void Update() {
		health_text.text = unit.status.Health.ToString();
	}
}