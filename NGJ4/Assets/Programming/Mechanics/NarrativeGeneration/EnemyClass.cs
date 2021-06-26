

public enum EnemyClass {
	CORRUPTED = 0,
	ZEALOT = 1,
	SCAVENGER = 2
}

public static class EnemyClass_Extensions {
	public static string GenAttribute(this EnemyClass enemy) =>
		(new string[]{"#enemyc", "#enemyz", "enemys"})[(int) enemy];
}
