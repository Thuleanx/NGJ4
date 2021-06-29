using UnityEngine;
using Thuleanx.Math;

public class ActivateOnce : MonoBehaviour {
	Timer timeLeft;
	void OnEnable() {
		float longest = 0;
		foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>()) {
			longest = Mathf.Max(longest, ps.main.duration + ps.main.startDelay.constant);
			ps.Play();
		}
		timeLeft = new Timer(longest);
		timeLeft.Start();
	}

	void Update() {
		if (!timeLeft) gameObject.SetActive(false);
	}
}