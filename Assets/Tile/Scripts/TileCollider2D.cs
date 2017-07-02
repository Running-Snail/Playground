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
        poly.points = RightAngleConvex(tilesBorder).ToArray();
	}
	
	// Update is called once per frame
	void Update () {
		
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

    private int FindMinAngleIdx(Vector2[] points, int curIdx, Vector2 edge) {
        int minAngleIdx = 0;
        float minAngle = 360f;
        Vector2 curPoint = points[curIdx];
        Debug.Log("--------------" + points[curIdx]);
        for (int i=0; i<points.Length; i++) {
            if (i == curIdx) {
                continue;
            }
            Vector2 p = points[i];
            float c = Vector2.Angle(p - curPoint, edge);

            // check is right angle
            Debug.Log("point " + p + " angle " + c);
            Debug.Log("check " + Mathf.Approximately(c, 0f) + "," + Mathf.Approximately(c, 90f) + "," + Mathf.Approximately(c, 180f));
            if ((Mathf.Approximately(c, 0f) || Mathf.Approximately(c, 90f) || Mathf.Approximately(c, 180f)) && c < minAngle) {
                minAngle = c;
                minAngleIdx = i;
            }
        }
        return minAngleIdx;
    }

    private int FindMinYIdx(Vector2[] points) {
        int minYIdx = 0;
        for (int i=1; i<points.Length; i++) {
            if (points[i].y < points[minYIdx].y) {
                minYIdx = i;
            }
        }
        return minYIdx;
    }

    private List<Vector2> RightAngleConvex(Vector2[] points) {
        List<Vector2> result = new List<Vector2>();
        int startIdx = FindMinYIdx(points); // find point with min y value
        int curIdx = startIdx;
        Vector2 curEdge = Vector2.right;
        result.Add(points[startIdx]);
        while (true) {
            int minAngleIdx = FindMinAngleIdx(points, curIdx, curEdge);
            Debug.Log("min idx " + minAngleIdx + ":" + points[minAngleIdx]);

            // end condition
            if (minAngleIdx == startIdx) {
                break;
            }
            curEdge = points[minAngleIdx] - points[curIdx];
            curIdx = minAngleIdx;

            // add to result set
            result.Add(points[curIdx]);
        }
        return result;
    }
}
