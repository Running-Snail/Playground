using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReflection : MonoBehaviour {
	public Renderer water;
	public Camera camera;
	public Vector2 speed = new Vector2(0.1f, 0f);

	private RenderTexture offscreenTexture;
	private Vector2 scroll;

	// Use this for initialization
	void Start () {
		offscreenTexture = new RenderTexture (1024, 1024, 16);
		scroll = water.material.mainTextureOffset;
		Vector3 size = water.bounds.size;
		float aspect = size.x / size.y;
		camera.aspect = aspect;
		camera.orthographicSize = 2.28f;
		camera.targetTexture = offscreenTexture;
	}
	
	// Update is called once per frame
	void Update () {
		if (water != null && camera != null) {
			scroll += speed * Time.deltaTime;
			// water.material.SetTextureOffset ("_NoiseTex", scroll);
			water.material.SetTexture ("_ReflectionTex", offscreenTexture);
		}
	}
}
