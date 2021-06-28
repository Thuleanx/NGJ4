using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterSidebarManager : MonoBehaviour {
	public GameObject SidebarDisplay;
	public List<KeyValuePair<PlayableUnit, CharacterSidebar>> sidebars 
		= new List<KeyValuePair<PlayableUnit, CharacterSidebar>>();

	public void AddUnit(PlayableUnit unit) {
		GameObject SidebarUnit = Instantiate(SidebarDisplay, 
			Vector2.zero, Quaternion.identity, gameObject.transform);
		RectTransform rectTransform = SidebarUnit.GetComponent<RectTransform>();
		if (rectTransform != null) {
			rectTransform.anchoredPosition3D = Vector3.zero;
			rectTransform.localScale = new Vector3(1,1,1);
		}
		sidebars.Add(new KeyValuePair<PlayableUnit, CharacterSidebar>(
			unit, SidebarUnit.GetComponentInChildren<CharacterSidebar>()
		));
	}
}