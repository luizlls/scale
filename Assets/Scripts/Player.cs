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


    [SerializeField]
    float scaleSpeed = 5f;

	[SerializeField]
    float scaleFactor = 1f;

	[SerializeField]
    float minScale = 0.50f;

	[SerializeField]
    float maxScale = 2.50f;

    [SerializeField]
    bool rotate = true;

    [SerializeField]
    float rotateDuration = 1f;

    Vector2 lastTouchPosition = Vector2.zero;


    float lastGroundedTime = 0f;

    bool grounded;

    Rigidbody2D body;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x < Screen.width / 2) {
				Scale(Input.mousePosition);
			}

            if (Input.mousePosition.x > Screen.width / 2 && CanJump()) {
                Jump();
			}
        }

        if (body.velocity.y != 0f) {
            SmoothFallMotion();
        }
    }

    bool CanJump()
    {
        return grounded && (Time.time > lastGroundedTime + jumpCooldownTime);
    }

    void Jump()
    {
        body.velocity = Vector2.up * jumpForce * scaleFactor;
        lastGroundedTime = Time.time;

        if (rotate) {
            StartCoroutine(Rotate(new Vector3(0f, 0f, -90f)));
        }
    }

    void SmoothFallMotion()
    {
        body.velocity += Physics2D.gravity.y * Vector2.up * scaleFactor * smoothFallMultiplier * Time.deltaTime;
    }

    void Scale(Vector2 currentTouchPosition)
	{
		if (currentTouchPosition.y > lastTouchPosition.y) {
			scaleFactor += scaleSpeed * Time.deltaTime;
		} else if (currentTouchPosition.y < lastTouchPosition.y) {
			scaleFactor -= scaleSpeed * Time.deltaTime;
		} else {
			scaleFactor = transform.localScale.x;
		}
		scaleFactor = Mathf.Clamp(scaleFactor, minScale, maxScale);
		transform.localScale = new Vector3(scaleFactor, scaleFactor);

		lastTouchPosition = currentTouchPosition;
	}

    bool rotating;
    IEnumerator Rotate(Vector3 angle)
    {
        if (rotating) {
            yield return null;
        }

        rotating = true;

        var target = transform.eulerAngles + angle;
        var current = transform.eulerAngles;

        float counter = 0;
        while (counter < rotateDuration)
        {
            counter += Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(current, target, counter / rotateDuration);
            yield return null;
        }

        rotating = false;
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
