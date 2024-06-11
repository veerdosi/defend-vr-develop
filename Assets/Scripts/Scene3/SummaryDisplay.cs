using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SummaryDisplay : MonoBehaviour
{
    public Text summaryText; // Reference to a UI Text element in Scene4
    public HighscoreTable highscoreTable; // Reference to the HighscoreTable script

    void Start()
    {
        SessionData sessionData = ShotSummaryData.sessionData; // Get the actual session data
        DisplayShotSummary(sessionData);
        highscoreTable.ShowHighscoreTable(sessionData);
    }

    void DisplayShotSummary(SessionData sessionData)
    {
        summaryText.text = "";
        for (int i = 0; i < sessionData.Rounds.Count; i++)
        {
            RoundData round = sessionData.Rounds[i];
            summaryText.text += "Shot Number: " + (i + 1) + ", Region: " + round.GoalPosition + "\n";
        }
    }
}
