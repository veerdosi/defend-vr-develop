using UnityEngine;

public class VRController : MonoBehaviour
{
    public Transform ball;
    private Vector3 lastPosition;
    private bool movementStarted;
    private float movementStartTime;
    private float initiationTime;

    void Start()
    {
        lastPosition = transform.position;
        movementStarted = false;
    }

    void Update()
    {
        DetectMovementStart();
    }

    void DetectMovementStart()
    {
        float velocity = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        if (velocity > 0.1f && !movementStarted) // Threshold for detecting movement
        {
            movementStartTime = Time.time;
            movementStarted = true;
        }
    }

    public void OnBallContact()
    {
        if (movementStarted)
        {
            initiationTime = Time.time - movementStartTime;
            movementStarted = false;
            Debug.Log("Initiation Time: " + initiationTime);
        }
    }

    public float GetInitiationTime()
    {
        return initiationTime;
    }
}