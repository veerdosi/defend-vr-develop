using UnityEngine;

public class GameController : MonoBehaviour
{
    public EndGameUI endGameUI;
    public HighscoreTable highscoreTable;
    private GameData gameData;
    private DataSaver dataSaver;
    private SessionData currentSession;
    private int roundCount = 0;
    private const int totalRounds = 10;

    private void Awake()
    {
        dataSaver = new DataSaver();
        dataSaver.Initialize();
    }

    private void Start()
    {
        gameData = dataSaver.LoadGameData(); // Load previous game data
        currentSession = new SessionData();  // Start a new session
    }

    // Called by Goalkeeper when a round ends
    public void EndRound()
    {
        roundCount++;

        if (roundCount >= totalRounds)
        {
            EndGame();
        }
        else
        {
            // Prepare for the next round if there are rounds remaining
            // Reset necessary states or notify other components if needed
        }
    }

    // Call this method when the game ends
    public void EndGame()
    {
        gameData.AddSessionData(currentSession);  // Add the current session to the game data
        endGameUI.ShowSummary(currentSession);  // Show summary for the current session
        dataSaver.SaveGameData(gameData);  // Save the game data including all sessions
        highscoreTable.ShowHighscoreTable(currentSession);  // Display the high score table
    }

    // Example of how to simulate game ending
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Use a key press to simulate end of game
        {
            EndGame();
        }
    }

    public void RecordRoundData(string goalPosition, int score, float reflectTime, float errorDistance)
    {
        currentSession.AddRoundData(goalPosition, score, reflectTime, errorDistance);
    }
}
