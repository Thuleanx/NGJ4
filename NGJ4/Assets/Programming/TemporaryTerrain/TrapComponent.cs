using UnityEngine;

public class TrapComponent : MonoBehaviour {
	public bool Deployed;

	private void OnEnable() {
		Deployed = false;
	}
	public void Deploy() => Deployed = true;
	public void UnWind() => Deployed = false;
}