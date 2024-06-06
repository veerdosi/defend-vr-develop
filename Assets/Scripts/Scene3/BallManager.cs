using System.Collections;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public GameObject ballPrefab; // Prefab for the ball
    public Transform ballSpawnPoint; // The spawn point for the balls
    public GameObject goal; // Reference to the goal
    public float spawnInterval = 3.0f; // Time interval between spawns
    public float force = 15.0f; // Forward force applied to shoot the ball
    public float upwardForce = 5.0f; // Upward force applied to shoot the ball
    public Vector3 ballScale = new Vector3(4, 4, 4); // Scale of the ball

    void Start()
    {
        StartCoroutine(SpawnBall());
    }

    IEnumerator SpawnBall()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            GameObject ball = Instantiate(ballPrefab, ballSpawnPoint.position, ballSpawnPoint.rotation);
            ball.transform.localScale = ballScale;
            ShootBall(ball);
        }
    }

    void ShootBall(GameObject ball)
    {
        // Determine a random point within the goal to shoot the ball towards
        Vector3 goalSize = goal.GetComponent<Renderer>().bounds.size;
        Vector3 goalPosition = goal.transform.position;

        float randomX = Random.Range(goalPosition.x - goalSize.x / 2, goalPosition.x + goalSize.x / 2);
        float randomY = Random.Range(goalPosition.y - goalSize.y / 2, goalPosition.y + goalSize.y / 2);

        Vector3 targetPoint = new Vector3(randomX, randomY, goalPosition.z);

        // Calculate the direction towards the target point
        Vector3 direction = (targetPoint - ball.transform.position).normalized;

        // Apply a forward and upward force to the ball to simulate a projectile
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        Vector3 forceVector = direction * force + Vector3.up * upwardForce;
        ballRigidbody.AddForce(forceVector, ForceMode.Impulse);

        // Add the BallBehavior component to handle destruction and scoring
        ball.AddComponent<BallBehavior>().Initialize(goal, this);
    }

    // This method will be called when the ball reaches the goal
    public void BallScored()
    {
        // Implement scoring logic here
        Debug.Log("Goal!");
    }

    // This method will be called when the ball is intercepted by the glove or stick
    public void BallIntercepted()
    {
        // Implement interception logic here
        Debug.Log("Intercepted!");
    }
}

public class BallBehavior : MonoBehaviour
{
    private GameObject goal;
    private BallManager ballManager;
    private bool isIntercepted = false;

    public void Initialize(GameObject goalObject, BallManager manager)
    {
        goal = goalObject;
        ballManager = manager;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Glove") || other.CompareTag("HockeyStick"))
        {
            isIntercepted = true;
            ballManager.BallIntercepted();
            Destroy(gameObject); // Destroy the ball on interception
        }
    }

    void Update()
    {
        if (!isIntercepted && transform.position.z >= goal.transform.position.z)
        {
            ballManager.BallScored();
            Destroy(gameObject); // Destroy the ball when it reaches the goal
        }
    }
}
