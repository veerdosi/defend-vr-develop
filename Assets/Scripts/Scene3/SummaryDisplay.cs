using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SummaryDisplay : MonoBehaviour
{
    public Text summaryText; // Reference to a UI Text element in Scene4
    public HighscoreTable highscoreTable; // Reference to the HighscoreTable script

    void Start()
    {
        List<string> shotRegions = ShotSummaryData.shotRegions;
        DisplayShotSummary(shotRegions);
        CreateAndShowHighscoreTable(shotRegions);
    }

    void DisplayShotSummary(List<string> shotRegions)
    {
        summaryText.text = "";
        for (int i = 0; i < shotRegions.Count; i++)
        {
            summaryText.text += "Shot Number: " + (i + 1) + ", Region: " + shotRegions[i] + "\n";
        }
    }

    void CreateAndShowHighscoreTable(List<string> shotRegions)
    {
        // Assuming ReflectTime, Score, BodyArea, and ErrorDistance are generated or obtained elsewhere
        SessionData sessionData = new SessionData();

        for (int i = 0; i < shotRegions.Count; i++)
        {
            sessionData.AddRoundData(
                goalPosition: shotRegions[i],
                score: Random.Range(0, 100), // Example value
                reflectTime: Random.Range(1.0f, 5.0f), // Example value
                errorDistance: Random.Range(0.0f, 10.0f), // Example value
                bodyArea: "ExampleBodyArea" // Example value
            );
        }

        highscoreTable.ShowHighscoreTable(sessionData);
    }
}
