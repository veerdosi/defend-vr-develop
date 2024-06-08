using System.Collections;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public GameObject ballPrefab; // Prefab for the ball
    public Transform ballSpawnPoint; // The spawn point for the balls
    public GameObject goal; // Reference to the goal
    public float spawnInterval = 3.0f; // Time interval between spawns
    public float shotSpeed = 16.67f; // Speed of the shot in m/s
    public float upwardAngle = 10f; // Angle in degrees for upward force
    public Vector3 ballScale = new Vector3(10, 10, 10); // Scale of the ball
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
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

        // Calculate forward and upward forces
        Vector3 forwardForce = direction * shotSpeed;
        Vector3 upwardForce = Quaternion.Euler(upwardAngle, 0, 0) * Vector3.up * shotSpeed;

        // Apply the combined force to the ball's Rigidbody
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.AddForce(forwardForce + upwardForce, ForceMode.Impulse);

        // Add the BallBehavior component to handle destruction and scoring
        ball.AddComponent<BallBehavior>().Initialize(goal, scoreManager);
    }
}
public class BallBehavior : MonoBehaviour
{
    private GameObject goal;
    private ScoreManager scoreManager;
    private bool isIntercepted = false;

    public void Initialize(GameObject goalObject, ScoreManager manager)
    {
        goal = goalObject;
        scoreManager = manager;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Glove"))
        {
            isIntercepted = true;
            Destroy(gameObject); // Destroy the ball on interception
        }
    }

    void Update()
    {
        if (!isIntercepted && transform.position.z >= goal.transform.position.z)
        {
            scoreManager.AddScore(1); // Increment the score
            Destroy(gameObject); // Destroy the ball when it reaches the goal
        }
    }
}
