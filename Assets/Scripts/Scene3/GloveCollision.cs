using UnityEngine;

public class GloveCollision : MonoBehaviour
{
    public ScoreManager scoreManager;
    public int pointsPerHit = 10;

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
