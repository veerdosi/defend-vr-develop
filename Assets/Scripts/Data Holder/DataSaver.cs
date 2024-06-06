using System.IO;
using UnityEngine;

public class DataSaver
{
    private string filePath;

    public DataSaver()
    {
        filePath = Application.persistentDataPath + "/gameData.json";
    }

    public void SaveGameData(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, json);
    }

    public GameData LoadGameData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return new GameData();
    }
}
