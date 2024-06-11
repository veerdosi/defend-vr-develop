using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public GameData GameData { get; private set; } = new GameData();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void RecordRoundData(string goalPosition, int score, float initiationTime, float errorDistance, string bodyArea)
    {
        if (GameData.Sessions.Count == 0)
        {
            GameData.AddSessionData(new SessionData());
        }

        GameData.Sessions[GameData.Sessions.Count - 1].AddRoundData(goalPosition, score, initiationTime, errorDistance, bodyArea);
    }

    public void EndRound()
    {
        // go to scene 4
    }
}
