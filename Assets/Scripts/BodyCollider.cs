using UnityEngine;

public class BodyCollider : MonoBehaviour
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
            // Increment score
            ScoreManager.Instance.IncrementScore();

            // Destroy the ball
            Destroy(other.gameObject);
        }
    }
}
