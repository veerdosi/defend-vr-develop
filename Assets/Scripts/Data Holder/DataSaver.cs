using UnityEngine;
using System.IO;

public class DataSaver
{
    private string savePath;

    public void Initialize()
    {
        savePath = Path.Combine(Application.persistentDataPath, "GameData.json");
    }

    public void SaveGameData(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(savePath, json);
    }

    public GameData LoadGameData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return new GameData(); // Return a new GameData if no file exists
    }
}
