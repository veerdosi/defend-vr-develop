using UnityEngine;

public class BodyCollider : MonoBehaviour
{
    private BallBehavior ballSpawner;
    private float reflexStartTime;
    private bool isReflexTimeRecorded = false;

    private void Start()
    {
        ballSpawner = FindObjectOfType<BallBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            // Record the body part
            string bodyPart = gameObject.name;

            // Calculate reflex time
            float reflexTime = isReflexTimeRecorded ? Time.time - reflexStartTime : 3f; // default 3s

            // Calculate error distance (distance between the ball and the collider)
            float errorDistance = Vector3.Distance(other.transform.position, transform.position);

            // Create and add the GoalAttempt
            GoalAttempt attempt = new GoalAttempt(ballSpawner.spawnCount, true, other.transform.position, reflexTime, errorDistance, bodyPart);
            DataManager.Instance.AddGoalAttempt(attempt);

            // Destroy the ball
            Destroy(other.gameObject);

            // Reset reflex time flag
            isReflexTimeRecorded = false;
        }
    }

    public void RecordReflexStartTime()
    {
        reflexStartTime = Time.time;
        isReflexTimeRecorded = true;
    }
}
