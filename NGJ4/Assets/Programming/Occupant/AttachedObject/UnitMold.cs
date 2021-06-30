using UnityEngine;
using System;
using System.Collections.Generic;
using Thuleanx.Optimization;

[CreateAssetMenu(fileName = "Mold", menuName = "~/Unit/Mold", order = 0)]
public class UnitMold : ScriptableObject {
	public List<UnitAction> ActionsList = new List<UnitAction>();
	public int vision_range = 4;
	public int max_health = 4;
	public int movement_range = 3; 
	public int attack_damage = 1;
	public int attack_range = 1;
	public BubblePool effect;
	public CharacterClass charClass;
	public EnemyClass enemyClass;
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
		unit.range = movement_range;

		GameObject unitObj = Instantiate(AttachedPrefab, 
			worldPos, Quaternion.identity);
		unit.Attach(unitObj);
		UnitObject objComponent = unitObj.GetComponent<UnitObject>();
		objComponent.AttachUnit(unit);
		return unit;
	}

	public AIUnit Create_E(Vector2Int position, Vector2 worldPos) {
		WalkTowardsPawns unit = new WalkTowardsPawns(position);
		unit.info = EnemyInfo.GenerateEnemy(enemyClass);
		unit.vision = vision_range;
		unit.characterSprite = CharacterSprite;
		unit.status.MaxHealth = max_health;
		unit.status.Health = max_health;
		unit.range = movement_range;
		unit.attack_damage = attack_damage;
		unit.attack_range = attack_range;
		unit.effect = effect;

		GameObject unitObj = Instantiate(AttachedPrefab, 
			worldPos, Quaternion.identity);
		unit.Attach(unitObj);
		UnitObject objComponent = unitObj.GetComponent<UnitObject>();
		objComponent.AttachUnit(unit);

		return unit;
	}
}