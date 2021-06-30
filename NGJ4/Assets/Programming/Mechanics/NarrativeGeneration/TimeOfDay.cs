using UnityEngine;

public enum TimeOfDay {
	DAWN = 0,
	NOON = 1,
	DUSK = 2,
	NIGHT = 3
}

public static class TimeOfDay_Extensions {
	public static string Name(this TimeOfDay time) =>
		(new string[]{"Dawn", "Noon", "Dusk", "Night"})[(int) time];
}
