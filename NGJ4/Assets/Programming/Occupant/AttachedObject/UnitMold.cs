using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Mold", menuName = "~/Unit/Mold", order = 0)]
public class UnitMold : ScriptableObject {
	public List<UnitAction> ActionsList = new List<UnitAction>();
	public int vision_range = 4;
	public int max_health = 4;
	public CharacterClass charClass;
	public Sprite CharacterSprite;
	public GameObject AttachedPrefab;

	public PlayableUnit Create(Vector2Int position, Vector2 worldPos) {
		PlayableUnit unit = new PlayableUnit(position);
		foreach (UnitAction uaction in ActionsList)
			unit.AddAction(uaction);
		unit.info = CharacterInfo.GenerateCharacter(charClass);
		unit.vision = vision_range;
		unit.characterSprite = CharacterSprite;
		unit.status.MaxHealth = max_health;
		unit.status.Health = max_health;

		GameObject unitObj = Instantiate(AttachedPrefab, 
			worldPos, Quaternion.identity);
		unit.Attach(unitObj);
		UnitObject objComponent = unitObj.GetComponent<UnitObject>();
		objComponent.AttachUnit(unit);

		return unit;
	}
}