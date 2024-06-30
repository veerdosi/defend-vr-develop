using UnityEngine;

public class Goal : MonoBehaviour
{
    private BallBehavior ballSpawner;

    private void Start()
    {
        ballSpawner = FindObjectOfType<BallBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            // Record the goal event
            string bodyPart = "None";
            float reflexTime = 3f;
            float errorDistance = 0f;

            // Create and add the GoalAttempt
            GoalAttempt attempt = new GoalAttempt(ballSpawner.spawnCount, false, other.transform.position, reflexTime, errorDistance, bodyPart);
            DataManager.Instance.AddGoalAttempt(attempt);

            // Destroy the ball
            Destroy(other.gameObject);
        }
    }
}
