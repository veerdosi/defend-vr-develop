using UnityEngine;

public class BodyCollider : MonoBehaviour
{
    private BallBehavior ballSpawner;
    private float reflexStartTime;

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
            float reflexTime = Time.time - reflexStartTime; // Calculate reflex time from start to collision

            // Calculate error distance
            float errorDistance = 0f; // Error distance is 0 if the ball is saved

            // Create and add the GoalAttempt
            GoalAttempt attempt = new GoalAttempt(ballSpawner.spawnCount + 1, true, ballSpawner.currentGoalPosition, reflexTime, errorDistance, bodyPart);
            DataManager.Instance.AddGoalAttempt(attempt);

            // Destroy the ball
            Destroy(other.gameObject);
        }
    }

    public void RecordReflexStartTime()
    {
        reflexStartTime = Time.time;
    }
}
