using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public enum GoalPosition
{
    TopLeft,
    TopMiddle,
    TopRight,
    BottomLeft,
    BottomMiddle,
    BottomRight
}

public class BallBehavior : MonoBehaviour
{
    public GameObject ballPrefab; // Assign the ball prefab in the inspector
    public GameObject GoalPrefab; // Assign the fake goal game object in the inspector
    public float spawnInterval = 3f; // Interval in seconds between spawns
    public float shootingForce = 10f; // Force applied to shoot the ball
    public Vector3 ballScale = new Vector3(2f, 2f, 2f); // Scale of the spawned ball
    public float delayBeforeShoot = 0; // Delay before shooting the ball
    public AudioClip spawnSound; // Assign the spawn sound clip in the inspector
    private BoxCollider goalCollider;
    private GoalPosition currentGoalPosition;
    private AudioSource audioSource;
    private int spawnCount = 0;
    private const int maxSpawnCount = 10;

    // Define target zones within the goal collider for each GoalPosition
    private Vector3[,] targetZones = new Vector3[6, 2]; // 6 is the number of elements in GoalPosition enum

    void Start()
    {
        goalCollider = GoalPrefab.GetComponent<BoxCollider>();
        if (goalCollider == null)
        {
            Debug.LogError("GoalPrefab does not have a BoxCollider component.");
            return;
        }

        // Define target zones based on goal collider bounds
        SetupTargetZones();

        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();

        StartCoroutine(SpawnBallRoutine());
    }

    void SetupTargetZones()
    {
        // TopLeft
        targetZones[(int)GoalPosition.TopLeft, 0] = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.TopLeft, 1] = new Vector3(goalCollider.bounds.center.x, goalCollider.bounds.max.y, GoalPrefab.transform.position.z);

        // TopMiddle
        targetZones[(int)GoalPosition.TopMiddle, 0] = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.TopMiddle, 1] = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.max.y, GoalPrefab.transform.position.z);

        // TopRight
        targetZones[(int)GoalPosition.TopRight, 0] = new Vector3(goalCollider.bounds.center.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.TopRight, 1] = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.max.y, GoalPrefab.transform.position.z);

        // BottomLeft
        targetZones[(int)GoalPosition.BottomLeft, 0] = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.min.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.BottomLeft, 1] = new Vector3(goalCollider.bounds.center.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);

        // BottomMiddle
        targetZones[(int)GoalPosition.BottomMiddle, 0] = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.min.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.BottomMiddle, 1] = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);

        // BottomRight
        targetZones[(int)GoalPosition.BottomRight, 0] = new Vector3(goalCollider.bounds.center.x, goalCollider.bounds.min.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.BottomRight, 1] = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);
    }

    IEnumerator SpawnBallRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        while (spawnCount < maxSpawnCount)
        {
            SpawnAndShootBall();
            //ScoreManager.Instance.IncrementScore();
            yield return new WaitForSeconds(spawnInterval);
            spawnCount++;
        }
        Application.Quit(); // Quit the application after 10 spawns
    }

    public void SpawnBall()
    {
        if (spawnCount < maxSpawnCount)
        {
            StartCoroutine(DelayedSpawnAndShootBall());
            spawnCount++;
        }
    }

    IEnumerator DelayedSpawnAndShootBall()
    {
        yield return new WaitForSeconds(3f); // 3-second delay before spawning a new ball
        SpawnAndShootBall();
    }

    void SpawnAndShootBall()
    {
        StartCoroutine(SpawnAndShootBallRoutine());
    }

    IEnumerator SpawnAndShootBallRoutine()
    {
        // Use the position of the BallSpawner as the spawn position
        Vector3 spawnPosition = transform.position;

        // Instantiate the ball at the spawn position
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        // Play the spawn sound
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }

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

        // Randomly select a goal position
        currentGoalPosition = (GoalPosition)Random.Range(0, 6);
        Debug.Log("Current Goal Position: " + currentGoalPosition);

        // Calculate target position based on current goal position
        Vector3 targetPosition = Vector3.Lerp(targetZones[(int)currentGoalPosition, 0], targetZones[(int)currentGoalPosition, 1], Random.value);

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
