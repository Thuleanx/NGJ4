using UnityEngine;
using System.Linq;

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

	public double Next() => random.NextDouble();

	public static int RangeWeighted(int[] weights) {
		int sum = weights.Aggregate(0, (cur, nxt) => cur + nxt, res => res);
		float pos = Random.Range(0f, sum);
		for (int i = 0; i < weights.Length; pos -= weights[i++])
			if (pos < weights[i]) return i;
		return weights.Length-1;
	}
}