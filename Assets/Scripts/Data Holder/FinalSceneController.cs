using UnityEngine;
using System.Collections.Generic;


public class FinalSceneController : MonoBehaviour
{
    public HighscoreTable highscoreTable;

    void Start()
    {
        if (highscoreTable != null)
        {
            List<GoalAttempt> goalAttempts = DataManager.Instance.GetGoalAttempts();
            highscoreTable.ShowHighscoreTable(goalAttempts);
        }
        else
        {
            Debug.LogError("HighscoreTable is not assigned in the inspector.");
        }
    }
}