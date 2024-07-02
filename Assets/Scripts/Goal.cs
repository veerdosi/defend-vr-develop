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

            // Calculate reflex time
            float reflexTime = Time.time - ballSpawner.ballStartTime; // Calculate reflex time from start to crossing the goal line

            // Calculate error distance (distance from ball to nearest body part)
            float errorDistance = float.MaxValue;
            foreach (BodyCollider bodyCollider in FindObjectsOfType<BodyCollider>())
            {
                float distance = Vector3.Distance(other.transform.position, bodyCollider.transform.position);
                if (distance < errorDistance)
                {
                    errorDistance = distance;
                }
            }

            // Create and add the GoalAttempt
            GoalAttempt attempt = new GoalAttempt(ballSpawner.spawnCount + 1, false, ballSpawner.currentGoalPosition, reflexTime, errorDistance, bodyPart);
            DataManager.Instance.AddGoalAttempt(attempt);

            // Destroy the ball
            Destroy(other.gameObject);
        }
    }
}
