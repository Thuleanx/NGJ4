using UnityEngine;

[System.Serializable]
public enum BiomeType {
	FOREST = 0,
	RUIN = 1,
	TENT_CITY = 2,
	SHRINE = 3,
	METEORITE = 4
}

[CreateAssetMenu(fileName = "Biome", menuName = "~/Map/Biome", order = 0)]
public class Biome : ScriptableObject {
	public string biomeName;
	public BiomeType biomeType;
}