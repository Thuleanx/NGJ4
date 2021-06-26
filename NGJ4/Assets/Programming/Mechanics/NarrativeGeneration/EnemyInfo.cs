using UnityEngine;
using Thuleanx;

public class EnemyInfo {
	public string trait;
	public EnemyClass enemyType;

	EnemyInfo(
		string trait,
		EnemyClass enemyType) {

		this.trait = trait;
		this.enemyType = enemyType;
	}

	public static EnemyInfo GenerateEnemy(EnemyClass enemyType) {
		string trait = App.Instance._NarrativeGenerator.parse(enemyType.GenAttribute());

		return new EnemyInfo(
			trait,
			enemyType
		);
	}
}