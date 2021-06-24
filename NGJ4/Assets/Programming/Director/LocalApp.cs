using UnityEngine;

public class LocalApp : MonoBehaviour {
	public VisionTracker _VisionTracker;

	private void Awake() {
		if (_VisionTracker == null) 
			_VisionTracker = GetComponentInChildren<VisionTracker>();
	}
}