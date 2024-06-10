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

    void Start()
    {
        // Assuming the goal prefab has a width and height matching a standard goal
        Renderer goalRenderer = goalPrefab.GetComponent<Renderer>();
        regionWidth = goalRenderer.bounds.size.x / 3;
        regionHeight = goalRenderer.bounds.size.y / 2;

        gameData = FindObjectOfType<GameController>().GameData;
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

            if (shotNumber >= 10)
            {
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

    void SendShotSummaryToNextScene()
    {
        ShotSummaryData.shotRegions = shotRegions;
        SceneManager.LoadScene("Scene4");
    }
}
