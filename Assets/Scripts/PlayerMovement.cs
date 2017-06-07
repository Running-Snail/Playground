using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
	public float speed = 5f;
    public float jumpSpeed = 5f;
    public bool inAir = true;

    private Rigidbody2D rb;
	private SpriteRenderer sr;
    private int groundLayerMask;
    private int groundLayerMaskBit;

    // Use this for initialization
    void Awake() {
        groundLayerMask = LayerMask.NameToLayer("Ground");
        groundLayerMaskBit = 1 << groundLayerMask;
    }

    void Start () {
		rb = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
	}

	void FixedUpdate() {
		float movement = Input.GetAxis ("Horizontal");
        float velocityY = rb.velocity.y;
        float velocityX = movement * speed;
        // jump?
        if (Input.GetKeyDown(KeyCode.Space) && inAir == false) {
            velocityY += jumpSpeed;
        }
		Vector2 newVelocity = new Vector2 (velocityX, velocityY);
		rb.velocity = newVelocity;

		// flip renderer
		if (sr) {
			if (movement < 0) {
				sr.flipX = true;
			} else {
				sr.flipX = false;
			}
		}
        //Debug.DrawLine(transform.position, transform.position + Vector3.down, Color.red);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == groundLayerMask) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayerMaskBit);
            if (hit) {
                Debug.Log("[OnCollisionEnter2D] hit " + hit.collider.name);
                inAir = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == groundLayerMask) {
            inAir = true;
        }
    }
}
