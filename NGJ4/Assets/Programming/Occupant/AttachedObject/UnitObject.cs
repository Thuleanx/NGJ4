using UnityEngine;
using Thuleanx.Optimization;
using TMPro;
using Thuleanx;

public class UnitObject : MonoBehaviour {
	Unit unit;
	public TMP_Text health_text;
	public ParticleCombo LandCombo;
	public BubblePool DeathCombo;

	[SerializeField, FMODUnity.EventRef] string moveRef;
	[SerializeField, FMODUnity.EventRef] string deadRef;
	[SerializeField, FMODUnity.EventRef] string attackRef;

	public void AttachUnit(Unit unit) {
		this.unit = unit;
	}

	public void Move(Vector2 position) {
		transform.position = position;
		LandCombo?.Activate();
		App.Instance._AudioManager.PlayOneShot(moveRef);
	}

	public void OnDeath() {
		DeathCombo?.Borrow(transform.position, Quaternion.identity);
		gameObject.SetActive(false);
		App.Instance._AudioManager.PlayOneShot(deadRef);
	}

	public void OnAttack() {
		App.Instance._AudioManager.PlayOneShot(attackRef);
	}

	void Update() {
		health_text.text = unit.status.Health.ToString();
	}
}