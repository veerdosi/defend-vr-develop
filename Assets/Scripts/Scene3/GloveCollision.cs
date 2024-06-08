using UnityEngine;

public class GloveCollision : MonoBehaviour
{
    private ScoreManager scoreManager;
    public int pointsPerHit = 10;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Ball hit the glove!");
            // Add points to the score
            if (scoreManager != null)
            {
                scoreManager.AddScore(pointsPerHit);
            }
        }
    }
}
