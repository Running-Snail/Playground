using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour {
    public float speed = 1.0f;

    private Renderer m_Renderer;

	// Use this for initialization
	void Start () {
        m_Renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        m_Renderer.material.SetTextureOffset("_NoiseTex", new Vector2(Time.time * speed, Time.time * speed));
	}
}
