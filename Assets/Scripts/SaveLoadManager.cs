using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    private static string filePath = Application.persistentDataPath + "/goalAttempts.json";

    [System.Serializable]
    public class GoalAttemptList
    {
        public List<GoalAttempt> goalAttempts;
    }

    public static void SaveData(List<GoalAttempt> goalAttempts)
    {
        GoalAttemptList goalAttemptList = new GoalAttemptList();
        goalAttemptList.goalAttempts = goalAttempts;

        string json = JsonUtility.ToJson(goalAttemptList, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved to " + filePath);
    }

    public static List<GoalAttempt> LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GoalAttemptList goalAttemptList = JsonUtility.FromJson<GoalAttemptList>(json);
            Debug.Log("Data loaded from " + filePath);
            return goalAttemptList.goalAttempts;
        }
        else
        {
            Debug.LogWarning("No save file found at " + filePath);
            return new List<GoalAttempt>();
        }
    }
}
