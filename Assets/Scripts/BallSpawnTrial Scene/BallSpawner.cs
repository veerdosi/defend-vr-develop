using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // Assign the ball prefab in the inspector
    public Transform fakeGoal; // Assign the fake goal transform in the inspector
    public float spawnInterval = 3f; // Interval in seconds between spawns
    public float spawnHeightOffset = 1f; // Height offset relative to the BallSpawner position
    public float shootingForce = 10f; // Force applied to shoot the ball
    public Vector3 ballScale = new Vector3(2f, 2f, 2f); // Scale of the spawned ball
    public float delayBeforeShoot = 3f; // Delay before shooting the ball

    private BoxCollider goalCollider;

    void Start()
    {
        goalCollider = fakeGoal.GetComponent<BoxCollider>();
        if (goalCollider == null)
        {
            Debug.LogError("FakeGoal does not have a BoxCollider component.");
            return;
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
        // this line is for random ball spawn position
        float lineStart = transform.position.x - goalCollider.size.x / 2f;
        float lineEnd = transform.position.x + goalCollider.size.x / 2f;

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
        float goalX = Random.Range(goalCollider.bounds.min.x, goalCollider.bounds.max.x);
        float goalY = Random.Range(goalCollider.bounds.min.y, goalCollider.bounds.max.y);
        Vector3 targetPosition = new Vector3(goalX, goalY, fakeGoal.position.z);

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
