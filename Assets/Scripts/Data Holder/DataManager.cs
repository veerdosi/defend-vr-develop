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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGoalAttempt(GoalAttempt attempt)
    {
        goalAttempts.Add(attempt);
    }

    public List<GoalAttempt> GetGoalAttempts()
    {
        return goalAttempts;
    }
}

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}