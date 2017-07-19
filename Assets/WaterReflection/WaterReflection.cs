﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaterReflection : MonoBehaviour {
	public Renderer water;
	public Camera camera;

	private RenderTexture offscreenTexture;

	// Use this for initialization
	void Start () {
		offscreenTexture = new RenderTexture (1024, 1024, 16);
		Vector3 size = water.bounds.size;
		float aspect = size.x / size.y;
		camera.aspect = aspect;
		camera.orthographicSize = size.y / 2f;
		camera.targetTexture = offscreenTexture;
        water.sharedMaterial.SetTexture("_ReflectionTex", offscreenTexture);
    }
}
