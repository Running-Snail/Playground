using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
	public float speed = 5f;

	private Rigidbody2D rb;
	private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
	}

	void FixedUpdate() {
		float movement = Input.GetAxis ("Horizontal");
		Vector2 newVelocity = new Vector2 (movement * speed, rb.velocity.y);
		rb.velocity = newVelocity;

		// flip renderer
		if (sr) {
			if (movement < 0) {
				sr.flipX = true;
			} else {
				sr.flipX = false;
			}
		}
	}
}
