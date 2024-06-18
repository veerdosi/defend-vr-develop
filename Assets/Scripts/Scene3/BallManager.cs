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
    public Vector3 ballScale = new Vector3(1, 1, 1); // Scale of the ball
    public AudioClip spawnSound; // Sound to play when ball spawns

    private bool isPaused = false; // Pause state
    private Coroutine spawnCoroutine;
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Start spawning balls
        spawnCoroutine = StartCoroutine(SpawnBallCoroutine());
    }

    IEnumerator SpawnBallCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (!isPaused)
            {
                SpawnAndShootBall();
            }
        }
    }

    void SpawnAndShootBall()
    {
        GameObject ball = Instantiate(ballPrefab, ballSpawnPoint.position, ballSpawnPoint.rotation);
        ball.transform.localScale = ballScale;
        Debug.Log("Ball instantiated at position: " + ball.transform.position);

        // Play the spawn sound
        audioSource.PlayOneShot(spawnSound);

        // Determine a random point within the goal to shoot the ball towards
        Vector3 goalSize = goal.GetComponent<Renderer>().bounds.size;
        Vector3 goalPosition = goal.transform.position;

        float randomX = Random.Range(goalPosition.x - goalSize.x / 2, goalPosition.x + goalSize.x / 2);
        float randomY = Random.Range(goalPosition.y - goalSize.y / 2, goalPosition.y + goalSize.y / 2);

        Vector3 targetPoint = new Vector3(randomX, randomY, goalPosition.z);
        Vector3 direction = (targetPoint - ball.transform.position).normalized;

        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        Debug.Log("Applying force towards: " + targetPoint);
        ballRigidbody.velocity = direction * shotSpeed;

        // Initialize BallBehavior component
        BallBehavior ballBehavior = ball.AddComponent<BallBehavior>();
        ballBehavior.Initialize(goal, FindObjectOfType<ScoreManager>());
    }

    public void TogglePausePlay()
    {
        isPaused = !isPaused;
    }
}
