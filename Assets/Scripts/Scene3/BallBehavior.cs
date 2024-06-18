using System;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    private GameObject goal;
    private ScoreManager scoreManager;
    private bool isIntercepted = false;
    private float movementStartTime;
    private bool movementStarted = false;
    private float initiationTime;
    private float reflectTime;
    private float startTime;

    public static event Action<Vector3, string> OnGoalScored;
    public static event Action<float, float, string> OnBallIntercepted;

    public void Initialize(GameObject goalObject, ScoreManager manager)
    {
        goal = goalObject;
        scoreManager = manager;
        startTime = Time.time; // Start time when the ball is instantiated
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Glove"))
        {
            isIntercepted = true;
            Destroy(gameObject); // Destroy the ball on interception

            // Calculate and log initiation time and reflect time
            initiationTime = Time.time - movementStartTime;
            reflectTime = Time.time - startTime;
            string bodyArea = other.name; // Assuming the glove name gives the body area

            OnBallIntercepted?.Invoke(initiationTime, reflectTime, bodyArea); // Notify subscribers
        }
    }

    void Update()
    {
        if (!isIntercepted && transform.position.z >= goal.transform.position.z)
        {
            string goalPosition = DetermineGoalPosition(); // Function to determine goal position
            OnGoalScored?.Invoke(transform.position, goalPosition); // Notify subscribers
            scoreManager.AddScore(1); // Increment the score
            Destroy(gameObject); // Destroy the ball when it reaches the goal
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isIntercepted && !collision.gameObject.CompareTag("Glove"))
        {
            // Destroy the ball if it collides with any object other than the glove
            Destroy(gameObject);
        }

        // Detect movement start
        if (!movementStarted && collision.gameObject.CompareTag("Glove"))
        {
            movementStartTime = Time.time;
            movementStarted = true;
        }
    }

    public float GetInitiationTime()
    {
        return initiationTime;
    }

    public float GetReflectTime()
    {
        return reflectTime;
    }

    private string DetermineGoalPosition()
    {
        // Implement logic to determine the position within the goal
        return "GoalPosition"; // Placeholder
    }
}
