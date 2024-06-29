using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public Rigidbody ballRigidbody;
    public Transform BallPlace;
    public float throwForceMultiplier = 0.1f; // Adjust this as necessary
    private Vector2 touchStartPosition;
    private Vector2 touchEndPosition;
    private bool isThrown = false;

    public float timeInterval = 4;
    private float timeCounter = 0;
    private bool shouldDrawLine = false;
    private Vector3 startLineWorldPosition;
    private Vector3 endLineWorldPosition;

    private Transform originalParent;

    void OnEnable()
    {
        RespawnBall();
        PlaceObjectOnPlane.HoopPlaced += EnableBall;
    }

    private void EnableBall()
    {
        this.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        PlaceObjectOnPlane.HoopPlaced -= DisableBall;
    }

    private void DisableBall()
    {
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        RespawnBall();
        originalParent = transform.parent;
    }

    void Update()
    {
        if (isThrown)
        {
            timeCounter += Time.deltaTime;
            if (ballRigidbody.velocity.magnitude < 0.1f || timeCounter >= timeInterval)
            {
                RespawnBall();
                isThrown = false;
            }
        }
        else
        {
            HandleInput();
        }

        if (shouldDrawLine)
        {
            Debug.DrawLine(startLineWorldPosition, endLineWorldPosition, Color.red);
        }
    }

    void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
                shouldDrawLine = false;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchEndPosition = touch.position;
                ProcessThrow();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            touchStartPosition = Input.mousePosition;
            shouldDrawLine = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touchEndPosition = Input.mousePosition;
            ProcessThrow();
        }
    }

    void ProcessThrow()
    {
        Vector2 direction = touchEndPosition - touchStartPosition;
        float magnitude = direction.magnitude;

        Debug.Log("Direction: " + direction + " Magnitude: " + magnitude);

        ThrowBallInDirection(direction, magnitude);
        isThrown = true;

        Vector3 startScreenPosition = new Vector3(touchStartPosition.x, touchStartPosition.y, Camera.main.nearClipPlane + 1f); // Adjust depth as needed
        Vector3 endScreenPosition = new Vector3(touchEndPosition.x, touchEndPosition.y, Camera.main.nearClipPlane + 1f);
        startLineWorldPosition = Camera.main.ScreenToWorldPoint(startScreenPosition);
        endLineWorldPosition = Camera.main.ScreenToWorldPoint(endScreenPosition);
        shouldDrawLine = true;

        Debug.Log("Start Line Position: " + startLineWorldPosition + " End Line Position: " + endLineWorldPosition);
    }

    void ThrowBallInDirection(Vector2 direction, float magnitude)
    {
        // Detach from parent (AR camera) before throwing
        transform.SetParent(null);

        // Normalize the screen direction vector and create a 3D direction vector
        Vector3 screenDirection = new Vector3(direction.x, direction.y, 0).normalized;

        // Get the direction relative to the camera's forward vector
        Vector3 worldDirection = Camera.main.transform.TransformDirection(screenDirection);
        worldDirection.z = 1; // Ensure the ball is thrown forward in the z-axis

        Vector3 force = worldDirection * (magnitude / 100f) * throwForceMultiplier;

        ballRigidbody.isKinematic = false;
        ballRigidbody.useGravity = true;
        ballRigidbody.AddForce(force, ForceMode.Impulse);
        Debug.Log("Force applied: " + force);
    }

    void RespawnBall()
    {
        timeCounter = 0;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.useGravity = false;
        ballRigidbody.isKinematic = true;

        // Reattach to the original parent (AR camera) when respawning
        transform.SetParent(originalParent);

        transform.position = BallPlace.position;
        Debug.Log("Ball respawned at: " + BallPlace.position);
    }
}