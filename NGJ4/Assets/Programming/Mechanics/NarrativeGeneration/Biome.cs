using UnityEngine;
using System;
using System.Collections.Generic;

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
			"#mvzs1",
			"#mvm1",
			"...",
		})[(int) biomeType];

	public static string MoveOut(this BiomeType biomeType) =>
		(new string[]{
			"#mvf2",
			"#mvr2", 
			"#mvt2",
			"#mvzs2",
			"#mvm2",
			"...",
		})[(int) biomeType];

	public static string MeteoriteCollect(this BiomeType biomeType) =>
		(new string[]{
			"#mpuf",
			"#mpur", 
			"#mputc",
			"#mpuzs",
			"#mpumz",
			"#mpug",
		})[(int) biomeType];

	public static float MeteoriteSpawnRateInverse(this BiomeType biomeType) =>
		(new float[]{
			200,
			30f, 
			60f,
			40f,
			20f,
			200f,
		})[(int) biomeType];

	public static int MeteoritePerChunk(this BiomeType biomeType) =>
		(new int[]{
			1,
			1, 
			1,
			1,
			2,
			1,
		})[(int) biomeType];

	public static float SpawnRateInverse(this BiomeType biomeType) =>
		(new float[]{
			200f,
			30f, 
			60f,
			40f,
			20f,
			400f,
		})[(int) biomeType];

	public static EnemyClass EnemySpawn(this BiomeType biomeType) {
		switch (biomeType) {
			case BiomeType.RUIN:
			case BiomeType.SHRINE:
				return EnemyClass.ZEALOT;
			case BiomeType.METEORITE:
				return EnemyClass.CORRUPTED;
			default:
				return EnemyClass.SCAVENGER;
		}
	}
}