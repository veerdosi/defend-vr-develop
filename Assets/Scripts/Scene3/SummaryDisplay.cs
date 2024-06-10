using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SummaryDisplay : MonoBehaviour
{
    public Text summaryText; // Reference to a UI Text element in Scene4

    void Start()
    {
        List<string> shotRegions = ShotSummaryData.shotRegions;
        DisplayShotSummary(shotRegions);
    }

    void DisplayShotSummary(List<string> shotRegions)
    {
        summaryText.text = "";
        for (int i = 0; i < shotRegions.Count; i++)
        {
            summaryText.text += "Shot Number: " + (i + 1) + ", Region: " + shotRegions[i] + "\n";
        }
    }
}
