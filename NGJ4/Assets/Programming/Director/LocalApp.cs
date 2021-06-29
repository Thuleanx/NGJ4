using UnityEngine;

public class LocalApp : MonoBehaviour {
	public VisionTracker _VisionTracker;
	public Narrator _Narrator;
	public LocalUIManager _UIManager;
	public CameraFollowUnit _Camera;

	private void Awake() {
		if (_VisionTracker == null) 
			_VisionTracker = GetComponentInChildren<VisionTracker>();
		if (_Narrator == null) 
			_Narrator = GetComponentInChildren<Narrator>();
		if (_UIManager == null) 
			_UIManager = GetComponentInChildren<LocalUIManager>();
		if (_Camera == null) 
			_Camera = GetComponentInChildren<CameraFollowUnit>();

	}
}