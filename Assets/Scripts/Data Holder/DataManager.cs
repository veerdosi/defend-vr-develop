using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private List<GoalAttempt> goalAttempts = new List<GoalAttempt>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGoalAttempt(GoalAttempt attempt)
    {
        goalAttempts.Add(attempt);
        SaveData();
    }

    public List<GoalAttempt> GetGoalAttempts()
    {
        return new List<GoalAttempt>(goalAttempts);
    }

    private void SaveData()
    {
        SaveLoadManager.SaveData(goalAttempts);
    }

    private void LoadData()
    {
        goalAttempts = SaveLoadManager.LoadData();
    }
}
