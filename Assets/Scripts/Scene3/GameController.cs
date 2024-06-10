using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameData GameData { get; private set; } = new GameData();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RecordRoundData(string goalPosition, int score, float reflectTime, float errorDistance, string bodyArea)
    {
        if (GameData.Sessions.Count == 0)
        {
            GameData.AddSessionData(new SessionData());
        }

        GameData.Sessions[GameData.Sessions.Count - 1].AddRoundData(goalPosition, score, reflectTime, errorDistance, bodyArea);
    }

    public void EndRound()
    {
        // Handle end of round logic here
        // For example, transition to the next scene or update the UI
    }
}
