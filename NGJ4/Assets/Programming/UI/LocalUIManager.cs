using UnityEngine;

public class LocalUIManager : MonoBehaviour {
	public AbilitySelector AbilitySelector;
	public SkipButton SkipButton;
	public CharacterSidebarManager SidebarManager;
	public CharacterPortrait Portrait;

	void Start() {
		SetInvisible();
	}

	public void SetInvisible() {
		GetComponent<Canvas>().enabled = false;
	}

	public void SetVisible() {
		GetComponent<Canvas>().enabled = true;
	}
}