using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GoalTracker : MonoBehaviour
{
    public GameObject goalPrefab; // Reference to the goal prefab

    private List<string> shotRegions = new List<string>();
    private int shotNumber = 0;
    private float regionWidth;
    private float regionHeight;
    private GameData gameData;
    private SessionData currentSession;

    void Start()
    {
        // Assuming the goal prefab has a width and height matching a standard goal
        Renderer goalRenderer = goalPrefab.GetComponent<Renderer>();
        regionWidth = goalRenderer.bounds.size.x / 3;
        regionHeight = goalRenderer.bounds.size.y / 2;

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameData = gameManager.GameData;
        }
        currentSession = new SessionData();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            shotNumber++;
            Vector3 ballPosition = other.transform.position;
            string region = GetGoalRegion(ballPosition);
            shotRegions.Add(region);
            Debug.Log("Shot Number: " + shotNumber + ", Region: " + region);

            BallBehavior ballBehavior = other.GetComponent<BallBehavior>();
            if (ballBehavior != null)
            {
                float initiationTime = ballBehavior.GetInitiationTime();
                float errorDistance = CalculateErrorDistance(ballPosition);
                string bodyArea = GetBodyArea(other.transform.position);

                currentSession.AddRoundData(region, 0, initiationTime, errorDistance, bodyArea);
            }

            if (shotNumber >= 10)
            {
                gameData.AddSessionData(currentSession);
                SendShotSummaryToNextScene();
            }
        }
    }

    string GetGoalRegion(Vector3 ballPosition)
    {
        Vector3 goalPosition = goalPrefab.transform.position;
        float relativeX = ballPosition.x - goalPosition.x + (goalPrefab.transform.localScale.x / 2);
        float relativeY = ballPosition.y - goalPosition.y;

        if (relativeY > regionHeight)
        {
            if (relativeX < regionWidth) return "Top Left";
            else if (relativeX < 2 * regionWidth) return "Top Center";
            else return "Top Right";
        }
        else
        {
            if (relativeX < regionWidth) return "Bottom Left";
            else if (relativeX < 2 * regionWidth) return "Bottom Center";
            else return "Bottom Right";
        }
    }

    float CalculateErrorDistance(Vector3 ballPosition)
    {
        GameObject[] controllers = GameObject.FindGameObjectsWithTag("VRController");
        float minDistance = float.MaxValue;

        foreach (GameObject controller in controllers)
        {
            float distance = Vector3.Distance(ballPosition, controller.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        return minDistance;
    }

    string GetBodyArea(Vector3 ballPosition)
    {
        // Implement logic to determine which body area the ball was aimed at or intercepted by
        // Placeholder return
        return "BodyArea";
    }

    void SendShotSummaryToNextScene()
    {
        ShotSummaryData.sessionData = currentSession; // Pass the actual session data
        SceneManager.LoadScene("Scene4");
    }
}
