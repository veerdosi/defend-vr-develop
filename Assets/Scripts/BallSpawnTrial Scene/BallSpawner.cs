using UnityEngine;
using System.Collections;

public enum GoalPosition
{
    TopLeft,
    TopMiddle,
    TopRight,
    BottomLeft,
    BottomMiddle,
    BottomRight
}
public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // Assign the ball prefab in the inspector
    public GameObject fakeGoal; // Assign the fake goal game object in the inspector
    public float spawnInterval = 3f; // Interval in seconds between spawns
    public float shootingForce = 10f; // Force applied to shoot the ball
    public Vector3 ballScale = new Vector3(2f, 2f, 2f); // Scale of the spawned ball
    public float delayBeforeShoot = 3f; // Delay before shooting the ball

    private BoxCollider goalCollider;
    private GoalPosition currentGoalPosition;

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
            // Randomly select a goal position
            currentGoalPosition = (GoalPosition)Random.Range(0, 6); // 6 is the number of elements in GoalPosition enum
            Debug.Log("Current Goal Position: " + currentGoalPosition);

            yield return StartCoroutine(SpawnAndShootBallRoutine());
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SpawnAndShootBallRoutine()
    {
        // Use the position of the BallSpawner as the spawn position
        Vector3 spawnPosition = transform.position;

        // Instantiate the ball at the spawn position
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        // Set the scale of the spawned ball
        ball.transform.localScale = ballScale;

        // Debug log to confirm ball instantiation
        Debug.Log("Ball instantiated at: " + spawnPosition);

        // Disable physics movement initially
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.isKinematic = true; // Make sure kinematics are on initially
            ballRb.useGravity = false; // Ensure gravity is off initially
        }
        else
        {
            Debug.LogError("Ball does not have a Rigidbody component.");
        }

        // Wait for the specified delay before shooting the ball
        yield return new WaitForSeconds(delayBeforeShoot);

        // Calculate target position based on current goal position
        Vector3 targetPosition = Vector3.zero;

        switch (currentGoalPosition)
        {
            case GoalPosition.TopLeft:
                targetPosition = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.max.y, fakeGoal.transform.position.z);
                break;
            case GoalPosition.TopMiddle:
                targetPosition = new Vector3((goalCollider.bounds.min.x + goalCollider.bounds.max.x) / 2f, goalCollider.bounds.max.y, fakeGoal.transform.position.z);
                break;
            case GoalPosition.TopRight:
                targetPosition = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.max.y, fakeGoal.transform.position.z);
                break;
            case GoalPosition.BottomLeft:
                targetPosition = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.min.y, fakeGoal.transform.position.z);
                break;
            case GoalPosition.BottomMiddle:
                targetPosition = new Vector3((goalCollider.bounds.min.x + goalCollider.bounds.max.x) / 2f, goalCollider.bounds.min.y, fakeGoal.transform.position.z);
                break;
            case GoalPosition.BottomRight:
                targetPosition = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.min.y, fakeGoal.transform.position.z);
                break;
        }

        // Debug log to confirm target position calculation
        Debug.Log("Target position calculated: " + targetPosition);

        // Calculate the direction towards the target point within the goal area
        Vector3 directionToGoal = (targetPosition - spawnPosition).normalized;

        // Apply force to the ball to shoot it towards the target point within the goal area
        if (ballRb != null)
        {
            ballRb.isKinematic = false; // Ensure kinematics are off
            ballRb.useGravity = true; // Ensure gravity is on

            ballRb.AddForce(directionToGoal * shootingForce, ForceMode.Impulse); // Adjust the force value as needed

            // Debug log to confirm force application
            Debug.Log("Force applied to ball: " + directionToGoal * shootingForce);
        }
        else
        {
            Debug.LogError("Ball does not have a Rigidbody component.");
        }
    }
}
