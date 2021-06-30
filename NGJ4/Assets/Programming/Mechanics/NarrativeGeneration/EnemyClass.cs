

public enum EnemyClass {
	CORRUPTED = 0,
	ZEALOT = 1,
	SCAVENGER = 2
}

public static class EnemyClass_Extensions {
	public static string GenAttribute(this EnemyClass enemy) =>
		(new string[]{"#enemyc", "#enemyz", "#enemys"})[(int) enemy];

	public static string Name(this EnemyClass enemy) =>
		(new string[]{"Corrupted", "Zealot", "Scavenger"})[(int) enemy];

	public static string DeathNarrative(this EnemyClass enemy, BiomeType biomeType) {
		if (enemy == EnemyClass.SCAVENGER) {
			switch (biomeType) {
				case BiomeType.FOREST:
					return "#fdw";
				case BiomeType.RUIN:
					return "#fdrc";
				case BiomeType.METEORITE:
					return "#fdm";
			}
		} else if (enemy == EnemyClass.CORRUPTED) {
			switch (biomeType) {
				case BiomeType.RUIN:
					return "#fdrs";
				case BiomeType.FOREST:
					return "#fdw";
			}
		} else if (enemy == EnemyClass.ZEALOT) {
			switch (biomeType) {
				case BiomeType.RUIN:
					return "#fdrz";
				case BiomeType.PLAIN:
					return "#fdez";
			}
		}

		return "#fde";
	}
}
