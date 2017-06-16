using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class TileCollider2D : MonoBehaviour {
	public float tileWidth = 1.0f;
	public float tileHeight = 1.0f;
	public Vector2[] tiles;

	private PolygonCollider2D poly;
	private Vector2[] tilesBorder;

	// Use this for initialization
	void Start () {
		poly = GetComponent<PolygonCollider2D>();
		tilesBorder = GenerateBorderPoints(tiles).ToArray();

		// check every point
		List<Vector2> colliderBorder = new List<Vector2>();
		foreach (Vector2 p in tilesBorder) {
			Debug.Log(p);
			bool inside = IsPointInside(tiles, p);
			Debug.Log(inside);
			if (!inside) {
				colliderBorder.Add(p);
			}
		}

		Debug.Log("---border---");

		foreach (Vector2 p in colliderBorder) {
			Debug.Log(p);
		}

		poly.points = colliderBorder.ToArray();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private bool IsPointInside(Vector2[] tiles, Vector2 p) {
		// 5 cases
		Vector2 p0 = new Vector2(p.x - tileWidth, p.y);
		Vector2 p1 = new Vector2(p.x, p.y);
		Vector2 p2 = new Vector2(p.x - tileWidth, p.y - tileHeight);
		Vector2 p3 = new Vector2(p.x, p.y - tileHeight);

		bool contains0 = ContainsPoint(tiles, p0);
		bool contains1 = ContainsPoint(tiles, p1);
		bool contains2 = ContainsPoint(tiles, p2);
		bool contains3 = ContainsPoint(tiles, p3);

		Debug.Log("---IsPointInside---");
		Debug.Log(p);
		Debug.Log(p0 + ":" + contains0 + "," + p1 + ":" + contains1 + "," + p2 + ":" + contains2 + "," + p3 + ":" + contains3);

		if (contains0 && contains1 && !contains2 && !contains3) {
			return true;
		} else if (contains0 && !contains1 && contains2 && !contains3) {
			return true;
		} else if (!contains0 && contains1 && !contains2 && contains3) {
			return true;
		} else if (!contains0 && !contains1 && contains2 && contains3) {
			return true;
		} else if (contains0 && contains1 && contains2 && contains3) {
			return true;
		}

		return false;
	}

	private List<Vector2> GenerateBorderPoints(Vector2[] tiles) {
		List<Vector2> result = new List<Vector2>();
		foreach (Vector2 p in tiles) {
			result.Add(new Vector2(p.x, p.y));
			result.Add(new Vector2(p.x + tileWidth, p.y));
			result.Add(new Vector2(p.x + tileWidth, p.y + tileHeight));
			result.Add(new Vector2(p.x, p.y + tileHeight));
		}
		return result;
	}

	private bool ContainsPoint(Vector2[] points, Vector2 point) {
		foreach (Vector2 p in points) {
			if (IsSamePoint(p, point)) {
				return true;
			}
		}
		return false;
	}

	private bool IsSamePoint(Vector2 a, Vector2 b) {
		return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
	}
}
