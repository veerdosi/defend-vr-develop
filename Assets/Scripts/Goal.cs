using UnityEngine;

public class Goal : MonoBehaviour
{
    private BallBehavior ballSpawner;

    private void Start()
    {
        ballSpawner = FindObjectOfType<BallBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            // Destroy the ball
            Destroy(other.gameObject);
        }
    }
}
