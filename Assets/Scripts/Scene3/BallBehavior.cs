using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

