using UnityEngine;
using Cinemachine;

public class CameraFollowUnit : MonoBehaviour {
	CinemachineVirtualCamera CVC;

	void Awake() {
		CVC = GetComponent<CinemachineVirtualCamera>();
	}

	public void FollowUnit(Unit unit) {
		CVC.Follow = unit.gameObject.transform;
	}
}