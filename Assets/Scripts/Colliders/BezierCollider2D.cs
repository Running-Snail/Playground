using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class BezierCollider2D : MonoBehaviour {
	public Vector2[] points;
	public float step = 0.05f;

	private Vector2 LinearInterpolate(Vector2 a, Vector2 b, float t) {
		return Vector2.Lerp (a, b, t);
	}

	private Vector2 Bezier(Vector2[] points, float t) {
		int count = points.Length;
		Vector2[] currentPoints = new Vector2[count];

		// copy
		for (int i = 0; i < points.Length; i++) {
			currentPoints [i] = points [i];
		}

		for (int i=0; i<points.Length; i++) {
			for (int j=0; j<=count-2; j++) {
				Vector2 start = currentPoints [j];
				Vector2 end = currentPoints [j+1];
				currentPoints [j] = LinearInterpolate (start, end, t);
			}
			count--;
		}

		// clear references?

		return currentPoints [0];
	}

	public Vector2[] EdgePoints() {
		int count = Mathf.FloorToInt (1f / step) + 1;
		float process = 0f;
		Vector2[] result = new Vector2[count];
		for (int i = 0; i < count; i++) {
			result [i] = Bezier (points, process);
			process += step;
		}
		return result;
	}
}
