using UnityEngine;

public class LocalApp : MonoBehaviour {
	public VisionTracker _VisionTracker;
	public Narrator _Narrator;

	private void Awake() {
		if (_VisionTracker == null) 
			_VisionTracker = GetComponentInChildren<VisionTracker>();
		if (_Narrator == null) 
			_Narrator = GetComponentInChildren<Narrator>();
	}
}