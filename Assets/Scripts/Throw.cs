using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Throw : MonoBehaviour {
    public Vector2 forceDir;
    public float power;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        if (rb) {
            rb.AddForce(forceDir.normalized * power, ForceMode2D.Impulse);
        }
	}
}
