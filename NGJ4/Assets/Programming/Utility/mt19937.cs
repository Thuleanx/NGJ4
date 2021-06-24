using UnityEngine;

public class mt19937 {
	System.Random random;

	public mt19937() {
		random = new System.Random();
	}

	public mt19937(int seed) {
		random = new System.Random(seed);
	}

	public int Range(int lo, int hi) {
		return (int) (random.NextDouble() * (hi-lo+1) + lo);
	}
}