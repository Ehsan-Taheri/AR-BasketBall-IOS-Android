using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public Rigidbody ballRigidbody;
    public float throwForceMultiplier = 10f;
    private Vector2 touchStartPosition;
    private Vector2 touchEndPosition;
    private bool isThrown = false;

    void Start()
    {
        // Ensure the ball starts at the bottom center of the screen
        RespawnBall();
    }

    void Update()
    {
        if (isThrown)
        {
            // Check if the ball is almost stopped
            if (ballRigidbody.velocity.magnitude < 0.1f)
            {
                RespawnBall();
                isThrown = false;
            }
        }
        else
        {
            HandleTouchInput();
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEndPosition = touch.position;

                Vector2 direction = touchEndPosition - touchStartPosition;
                float magnitude = direction.magnitude;

                ThrowBallInDirection(direction.normalized, magnitude);
                isThrown = true;
            }
        }
    }

    void ThrowBallInDirection(Vector2 direction, float magnitude)
    {
        Vector3 force = new Vector3(direction.x, direction.y, 0) * magnitude * throwForceMultiplier;
        ballRigidbody.isKinematic = false; // Enable physics
        ballRigidbody.AddForce(force, ForceMode.Impulse);
    }

    void RespawnBall()
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.isKinematic = true; // Disable physics

        Vector3 screenBottomCenter = new Vector3(Screen.width / 2, 0, Camera.main.nearClipPlane + 1);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenBottomCenter);
        transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
    }
}