using UnityEngine;
using Thuleanx.Optimization;
using TMPro;

public class UnitObject : MonoBehaviour {
	Unit unit;
	public TMP_Text health_text;
	public ParticleCombo LandCombo;
	public BubblePool DeathCombo;

	public void AttachUnit(Unit unit) {
		this.unit = unit;
	}

	public void Move(Vector2 position) {
		transform.position = position;
		LandCombo?.Activate();
	}

	public void OnDeath() {
		DeathCombo?.Borrow(transform.position, Quaternion.identity);
		gameObject.SetActive(false);
	}

	void Update() {
		health_text.text = unit.status.Health.ToString();
	}
}