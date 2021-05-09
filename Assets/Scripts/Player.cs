using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float smoothFallMultiplier = 2.5f;

    [SerializeField]
    float jumpForce = 5f;

    [SerializeField]
    float jumpCooldownTime = 0.5f;

    float lastGroundedTime = 0f;

    bool grounded;

    Rigidbody2D body;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanJump()) {
            Jump();
        }

        if (!grounded) {
            SmoothFallMotion(smoothFallMultiplier);
        }
    }

    bool CanJump()
    {
        return grounded && (Time.time > lastGroundedTime + jumpCooldownTime);
    }

    void Jump()
    {
        body.velocity = Vector2.up * jumpForce;
        lastGroundedTime = Time.time;
    }

    void SmoothFallMotion(float multiplier)
    {
        body.velocity += Vector2.up * Physics2D.gravity.y * (multiplier - 1) * Time.deltaTime;
    }

	void OnCollisionStay2D(Collision2D collider)
	{
        grounded = true;
 	}

	void OnCollisionExit2D(Collision2D collider)
	{
        grounded = false;
	}
}
