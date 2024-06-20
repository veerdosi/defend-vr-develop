using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // Assign the ball prefab in the inspector
    public Transform goal; // Assign the goal transform in the inspector
    public float spawnInterval = 3f; // Interval in seconds between spawns
    public float spawnHeightOffset = 1f; // Height offset relative to the BallSpawner position
    public float shootingForce = 10f; // Force applied to shoot the ball
    public Vector3 ballScale = new Vector3(2f, 2f, 2f); // Scale of the spawned ball
    public float delayBeforeShoot = 3f; // Delay before shooting the ball

    private float goalWidth;
    private float goalHeight;

    void Start()
    {
        GoalTracker goalTracker = goal.GetComponent<GoalTracker>();
        if (goalTracker != null)
        {
            goalWidth = goalTracker.GetRegionWidth() * 3; // Full goal width
            goalHeight = goalTracker.GetRegionHeight() * 2; // Full goal height
        }
        StartCoroutine(SpawnBallRoutine());
    }

    IEnumerator SpawnBallRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnAndShootBallRoutine());
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SpawnAndShootBallRoutine()
    {
        // Determine the start and end positions of the line relative to the BallSpawner's position
        float lineStart = transform.position.x - goalWidth / 2f;
        float lineEnd = transform.position.x + goalWidth / 2f;

        // Determine a random position along the specified line
        float randomX = Random.Range(lineStart, lineEnd);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y + spawnHeightOffset, transform.position.z);

        // Instantiate the ball at the random position
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        // Set the scale of the spawned ball
        ball.transform.localScale = ballScale;

        // Wait for the specified delay before shooting the ball
        yield return new WaitForSeconds(delayBeforeShoot);

        // Calculate a random target point within the goal area
        float goalX = Random.Range(goal.position.x - goalWidth / 2f, goal.position.x + goalWidth / 2f);
        float goalY = Random.Range(goal.position.y - goalHeight / 2f, goal.position.y + goalHeight / 2f);
        Vector3 targetPosition = new Vector3(goalX, goalY, goal.position.z);

        // Calculate the direction towards the target point within the goal area
        Vector3 directionToGoal = (targetPosition - spawnPosition).normalized;

        // Apply force to the ball to shoot it towards the target point within the goal area
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.AddForce(directionToGoal * shootingForce, ForceMode.Impulse); // Adjust the force value as needed
        }
    }
}
