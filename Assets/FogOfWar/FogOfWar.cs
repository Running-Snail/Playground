using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {
    public Renderer fogOfWarRenderer;
    public Transform follow;

    private Vector2 planeCenter = new Vector2(0.5f, 0.5f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = follow.transform.position;
        fogOfWarRenderer.material.SetVector(
            "_Center1", new Vector4(pos.x, pos.y, pos.z, 1f));
	}
}
