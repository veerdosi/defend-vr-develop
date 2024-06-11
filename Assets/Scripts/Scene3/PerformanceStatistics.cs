using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PerformanceStatistics : MonoBehaviour
{
    public GameObject statRowPrefab;
    public Transform contentArea;
    public Button playAgainButton;
    public Button homeButton;
    private DataSaver dataSaver;

    private void Start()
    {
        dataSaver = new DataSaver();
        playAgainButton.onClick.AddListener(PlayAgain);
        homeButton.onClick.AddListener(GoHome);
        DisplayStatistics();
    }

    private void DisplayStatistics()
    {
        GameData gameData = dataSaver.LoadGameData();
        if (gameData.Sessions.Count > 0)
        {
            SessionData latestSession = gameData.Sessions[gameData.Sessions.Count - 1];
            foreach (RoundData round in latestSession.Rounds)
            {
                GameObject row = Instantiate(statRowPrefab, contentArea);
                row.transform.GetChild(0).GetComponent<Text>().text = round.GoalPosition;
                row.transform.GetChild(1).GetComponent<Text>().text = round.Score.ToString();
                row.transform.GetChild(2).GetComponent<Text>().text = round.InitiationTime.ToString("F2") + "s";
                row.transform.GetChild(3).GetComponent<Text>().text = round.ErrorDistance.ToString("F2") + "m";
            }
        }
    }

    private void PlayAgain()
    {
        // Logic to restart the game
        SceneManager.LoadScene("scene3");
    }

    private void GoHome()
    {
        // Logic to go back to the home scene
        SceneManager.LoadScene("scene1");
    }
}
