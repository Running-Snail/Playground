using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCollider2D))]
[CanEditMultipleObjects]
public class BezierCollider2DEditor : Editor {
	private SerializedProperty points;
	private SerializedProperty step;
	private BezierCollider2D bezierCollider;
	private EdgeCollider2D edgeCollider;

	void OnEnable() {
		points = serializedObject.FindProperty ("points");
		step = serializedObject.FindProperty ("step");
		bezierCollider = (BezierCollider2D)target;
		edgeCollider = bezierCollider.GetComponent<EdgeCollider2D> ();
	}

	public override void OnInspectorGUI() {
		EditorGUILayout.PropertyField (points, true);
		EditorGUILayout.PropertyField (step);
		serializedObject.ApplyModifiedProperties ();
		edgeCollider.points = bezierCollider.EdgePoints ();
	}

	public virtual void OnSceneGUI() {
		EditorGUI.BeginChangeCheck ();
		Vector3[] newPositions = new Vector3[bezierCollider.points.Length];
		for (int i=0; i<bezierCollider.points.Length; i++) {
			newPositions[i] = Handles.FreeMoveHandle (bezierCollider.points[i], Quaternion.identity,
				0.1f, Vector3.one, Handles.DotHandleCap);
		}
		if (EditorGUI.EndChangeCheck ()) {
			for (int i = 0; i < bezierCollider.points.Length; i++) {
				bezierCollider.points[i] = new Vector2(newPositions[i].x, newPositions[i].y);
			}
			edgeCollider.points = bezierCollider.EdgePoints ();
		}
	}
}
