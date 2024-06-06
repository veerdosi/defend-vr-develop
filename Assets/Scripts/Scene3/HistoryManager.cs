using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class HistoryManager : MonoBehaviour
{
    public Dropdown recordingsDropdown;
    public Button playButton;
    private List<ReplaySystem.Recording> recordings = new List<ReplaySystem.Recording>();

    void Start()
    {
        LoadRecordings();
    }

    void LoadRecordings()
    {
        string path = Path.Combine(Application.persistentDataPath, "recordings.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            recordings = JsonUtility.FromJson<SerializationWrapper<List<ReplaySystem.Recording>>>(jsonData).data;
        }
        LoadDropdownOptions();
    }

    void LoadDropdownOptions()
    {
        recordingsDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (ReplaySystem.Recording rec in recordings)
        {
            options.Add(rec.timestamp);
        }
        recordingsDropdown.AddOptions(options);
    }

    public void PlaySelectedRecording()
    {
        int selectedIndex = recordingsDropdown.value;
        PlayerPrefs.SetInt("SelectedRecordingIndex", selectedIndex);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("ViewingScene");
    }

    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public T data;
        public SerializationWrapper(T data) { this.data = data; }
    }
}