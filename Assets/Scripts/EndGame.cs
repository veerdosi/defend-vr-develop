using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public GameObject summaryPanel;
    public Text summaryText; // Assign this in the Inspector

    public void ShowSummary(SessionData sessionData)
    {
        summaryPanel.SetActive(true);
        summaryText.text = GenerateSummaryText(sessionData);
    }

    private string GenerateSummaryText(SessionData sessionData)
    {
        string summary = "Position\tScore\tReflect Time\n";
        foreach (var round in sessionData.Rounds)
        {
            summary += $"{round.GoalPosition}\t{round.Score}\t{round.ReflectTime:F2}\n";
        }
        return summary;
    }
}
