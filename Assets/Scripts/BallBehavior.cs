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

public class BallBehavior : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject GoalPrefab;
    public float spawnInterval = 3f;
    public float shootingForce = 10f;
    public Vector3 ballScale = new Vector3(2f, 2f, 2f);
    public float delayBeforeShoot = 0;
    public AudioClip spawnSound;
    private BoxCollider goalCollider;
    private GoalPosition currentGoalPosition;
    private AudioSource audioSource;
    public int spawnCount = 0;
    private const int maxSpawnCount = 10;

    private Vector3[,] targetZones = new Vector3[6, 2];

    void Start()
    {
        goalCollider = GoalPrefab.GetComponent<BoxCollider>();
        if (goalCollider == null)
        {
            Debug.LogError("GoalPrefab does not have a BoxCollider component.");
            return;
        }

        SetupTargetZones();
        audioSource = gameObject.AddComponent<AudioSource>();
        StartCoroutine(SpawnBallRoutine());
    }

    void SetupTargetZones()
    {
        targetZones[(int)GoalPosition.TopLeft, 0] = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.TopLeft, 1] = new Vector3(goalCollider.bounds.center.x, goalCollider.bounds.max.y, GoalPrefab.transform.position.z);

        targetZones[(int)GoalPosition.TopMiddle, 0] = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.TopMiddle, 1] = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.max.y, GoalPrefab.transform.position.z);

        targetZones[(int)GoalPosition.TopRight, 0] = new Vector3(goalCollider.bounds.center.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.TopRight, 1] = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.max.y, GoalPrefab.transform.position.z);

        targetZones[(int)GoalPosition.BottomLeft, 0] = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.min.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.BottomLeft, 1] = new Vector3(goalCollider.bounds.center.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);

        targetZones[(int)GoalPosition.BottomMiddle, 0] = new Vector3(goalCollider.bounds.min.x, goalCollider.bounds.min.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.BottomMiddle, 1] = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);

        targetZones[(int)GoalPosition.BottomRight, 0] = new Vector3(goalCollider.bounds.center.x, goalCollider.bounds.min.y, GoalPrefab.transform.position.z);
        targetZones[(int)GoalPosition.BottomRight, 1] = new Vector3(goalCollider.bounds.max.x, goalCollider.bounds.center.y, GoalPrefab.transform.position.z);
    }

    IEnumerator SpawnBallRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        while (spawnCount < maxSpawnCount)
        {
            SpawnAndShootBall();
            yield return new WaitForSeconds(spawnInterval);
            spawnCount++;
        }
        Application.Quit();
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
        yield return new WaitForSeconds(3f);
        SpawnAndShootBall();
    }

    void SpawnAndShootBall()
    {
        StartCoroutine(SpawnAndShootBallRoutine());
    }

    IEnumerator SpawnAndShootBallRoutine()
    {
        Vector3 spawnPosition = transform.position;
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }

        ball.transform.localScale = ballScale;
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.isKinematic = true;
            ballRb.useGravity = false;
        }
        else
        {
            Debug.LogError("Ball does not have a Rigidbody component.");
        }

        yield return new WaitForSeconds(delayBeforeShoot);

        currentGoalPosition = (GoalPosition)Random.Range(0, 6);
        Vector3 targetPosition = Vector3.Lerp(targetZones[(int)currentGoalPosition, 0], targetZones[(int)currentGoalPosition, 1], Random.value);
        Vector3 directionToGoal = (targetPosition - spawnPosition).normalized;

        if (ballRb != null)
        {
            ballRb.isKinematic = false;
            ballRb.useGravity = true;
            ballRb.AddForce(directionToGoal * shootingForce, ForceMode.Impulse);

            // Record reflex start time for all body colliders
            foreach (BodyCollider bodyCollider in FindObjectsOfType<BodyCollider>())
            {
                bodyCollider.RecordReflexStartTime();
            }
        }
        else
        {
            Debug.LogError("Ball does not have a Rigidbody component.");
        }
    }
}
