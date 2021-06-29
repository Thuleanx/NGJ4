using UnityEngine;

[System.Serializable]
public enum BiomeType {
	FOREST = 0,
	RUIN = 1,
	TENT_CITY = 2,
	SHRINE = 3,
	METEORITE = 4,
	PLAIN = 5
}

[CreateAssetMenu(fileName = "Biome", menuName = "~/Map/Biome", order = 0)]
public class Biome : ScriptableObject {
	public string biomeName;
	public BiomeType biomeType;
	public string BiomeDescription;
}

public static class Biome_Extensions {
	public static string MoveIn(this BiomeType biomeType) =>
		(new string[]{
			"#mvf1",
			"#mvr1", 
			"#mvt1",
			"#mvs1",
			"#mvm1",
			"#mvb",
		})[(int) biomeType];

	public static string MoveOut(this BiomeType biomeType) =>
		(new string[]{
			"#mvf2",
			"#mvr2", 
			"#mvt2",
			"#mvs2",
			"#mvm2",
			"#mvb",
		})[(int) biomeType];
}