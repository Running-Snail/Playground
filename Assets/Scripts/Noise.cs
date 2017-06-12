using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {
	public static float Perlin(float x, float y, float z) {
		float xy = Mathf.PerlinNoise (x, y);
		float yz = Mathf.PerlinNoise (y, z);
		float xz = Mathf.PerlinNoise (x, z);

		// reverse
		float yx = Mathf.PerlinNoise (y, x);
		float zy = Mathf.PerlinNoise (z, y);
		float zx = Mathf.PerlinNoise (z, x);

		return xy + yz + xz + yx + zy + zx / 6.0f;
	}
}
