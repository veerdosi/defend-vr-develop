/*

using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameData gameData = new GameData();
    private SessionData currentSession = new SessionData();

    public GameData GameData => gameData;

    void Start()
    {
        // Subscribe to events
        BallBehavior.OnGoalScored += OnGoalScored;
        BallBehavior.OnBallIntercepted += OnBallIntercepted;
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        BallBehavior.OnGoalScored -= OnGoalScored;
        BallBehavior.OnBallIntercepted -= OnBallIntercepted;
    }

    void OnGoalScored(Vector3 ballPosition, string goalPosition)
    {
        // Calculate error distance
        GameObject leftHand = GameObject.Find("LeftHand");
        GameObject rightHand = GameObject.Find("RightHand");
        float minDistance = Mathf.Min(
            Vector3.Distance(ballPosition, leftHand.transform.position),
            Vector3.Distance(ballPosition, rightHand.transform.position)
        );

        Debug.Log("Error Distance: " + minDistance);

        // Add round data
        currentSession.AddRoundData(goalPosition, 0, 0, minDistance, "");
    }

    void OnBallIntercepted(float initiationTime, float reflectTime, string bodyArea)
    {
        Debug.Log("Initiation Time: " + initiationTime);
        Debug.Log("Reflect Time: " + reflectTime);

        // Add round data
        currentSession.AddRoundData("", 1, initiationTime, 0, bodyArea);
    }

    public void EndSession()
    {
        gameData.AddSessionData(currentSession);
        // Show highscore table
        FindObjectOfType<HighscoreTable>().ShowHighscoreTable(currentSession);
        currentSession = new SessionData(); // Reset for the next session
    }
}
*/