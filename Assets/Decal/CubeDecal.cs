using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDecal : MonoBehaviour {
    public Renderer Renderer;

	// Use this for initialization
	void Start () {
		if (Renderer == null) {
            Renderer = GetComponent<Renderer>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            Debug.DrawRay(hit.point, hit.normal, Color.red);
            Renderer.material.SetVector("_DecalCenter", new Vector4(hit.point.x, hit.point.y, hit.point.z, 0f));
        }
    }
}
