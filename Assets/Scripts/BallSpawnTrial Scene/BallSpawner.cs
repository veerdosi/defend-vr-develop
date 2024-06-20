using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // Assign the ball prefab in the inspector
    public Transform goal; // Assign the goal transform in the inspector
    public float spawnInterval = 3f; // Interval in seconds
    public float lineLength = 10f; // Total length of the line along which balls can spawn
    public float spawnHeightOffset = 1f; // Height offset relative to the BallSpawner position
    public float shootingForce = 10f; // Force applied to shoot the ball
    public Vector3 ballScale = new Vector3(2f, 2f, 2f); // Scale of the spawned ball
    public Vector3 goalAreaSize = new Vector3(5f, 5f, 0f); // Size of the goal area in X and Y dimensions

    void Start()
    {
        StartCoroutine(SpawnBall());
    }

    IEnumerator SpawnBall()
    {
        while (true)
        {
            SpawnAndShootBall();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnAndShootBall()
    {
        // Determine the start and end positions of the line relative to the BallSpawner's position
        float lineStart = transform.position.x - lineLength / 2f;
        float lineEnd = transform.position.x + lineLength / 2f;

        // Determine a random position along the specified line
        float randomX = Random.Range(lineStart, lineEnd);
        Vector3 spawnPosition = new Vector3(randomX, transform.position.y + spawnHeightOffset, transform.position.z);

        // Instantiate the ball at the random position
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        // Set the scale of the spawned ball
        ball.transform.localScale = ballScale;

        // Calculate a random target point within the goal area
        float goalX = Random.Range(goal.position.x - goalAreaSize.x / 2f, goal.position.x + goalAreaSize.x / 2f);
        float goalY = Random.Range(goal.position.y - goalAreaSize.y / 2f, goal.position.y + goalAreaSize.y / 2f);
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
