using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterPortrait : MonoBehaviour {
	[SerializeField] Image portrait;
	[SerializeField] TMP_Text text;

	void Start() { 
		gameObject.SetActive(false);
	}

	public void Register(PlayableUnit punit) {
		gameObject.SetActive(true);
		portrait.sprite = punit.characterSprite;
		text.text = punit.info.fullName;
	}

	public void Disable() {
		gameObject.SetActive(false);
	}


}