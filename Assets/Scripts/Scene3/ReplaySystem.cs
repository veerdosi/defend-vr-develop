using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ReplaySystem : MonoBehaviour
{
    public Camera thisCam;
    public float delayBetweenFrames = 0.05f;
    private List<Recording> recordings = new List<Recording>();
    private Recording currentRecording;
    private bool isRecording = false;
    private bool isPlaying = false;
    public Renderer quadRenderer;
    public Text recordingText;
    public Dropdown recordingsDropdown;

    void Start()
    {
        thisCam = GetComponent<Camera>();
        if (recordingsDropdown != null)
        {
            LoadRecordings();
        }
        else
        {
            LoadSelectedRecording();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecording();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopRecording();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartPlayback();
        }
    }

    public void StartRecording()
    {
        currentRecording = new Recording();
        isRecording = true;
        recordingText.text = "Recording...";
    }

    public void StopRecording()
    {
        isRecording = false;
        recordingText.text = "Stopped Recording";
        if (currentRecording.frames.Count > 0)
        {
            if (recordings.Count >= 10)
            {
                recordings.RemoveAt(0); // Remove the oldest recording if limit is reached
            }
            recordings.Add(currentRecording);
            SaveRecordings();
        }
    }

    void RecordFrame()
    {
        if (isRecording)
        {
            Texture2D frame = CaptureFrame();
            currentRecording.frames.Add(frame.EncodeToPNG());
        }
    }

    Texture2D CaptureFrame()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = thisCam.targetTexture;

        Texture2D frame = new Texture2D(thisCam.targetTexture.width, thisCam.targetTexture.height, TextureFormat.RGB24, false);
        frame.ReadPixels(new Rect(0, 0, thisCam.targetTexture.width, thisCam.targetTexture.height), 0, 0);
        frame.Apply();
        RenderTexture.active = currentRT;
        return frame;
    }

    void DisplayFrame(Texture2D frame)
    {
        quadRenderer.material.mainTexture = frame;
    }

    public void StartPlayback()
    {
        if (!isPlaying && recordingsDropdown.value < recordings.Count)
        {
            currentRecording = recordings[recordingsDropdown.value];
            StartCoroutine(Playback());
        }
    }

    void LateUpdate()
    {
        RecordFrame();
    }

    IEnumerator Playback()
    {
        isPlaying = true;
        recordingText.text = "Playing...";
        foreach (byte[] frameData in currentRecording.frames)
        {
            Texture2D frame = new Texture2D(2, 2);
            frame.LoadImage(frameData);
            DisplayFrame(frame);
            yield return new WaitForSeconds(delayBetweenFrames);
        }
        isPlaying = false;
        recordingText.text = "Stopped Playing";
    }

    void SaveRecordings()
    {
        string path = Path.Combine(Application.persistentDataPath, "recordings.json");
        string jsonData = JsonUtility.ToJson(new SerializationWrapper<List<Recording>>(recordings));
        File.WriteAllText(path, jsonData);
        LoadDropdownOptions();
    }

    void LoadRecordings()
    {
        string path = Path.Combine(Application.persistentDataPath, "recordings.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            recordings = JsonUtility.FromJson<SerializationWrapper<List<Recording>>>(jsonData).data;
        }
        LoadDropdownOptions();
    }

    void LoadSelectedRecording()
    {
        string path = Path.Combine(Application.persistentDataPath, "recordings.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            recordings = JsonUtility.FromJson<SerializationWrapper<List<Recording>>>(jsonData).data;
            int selectedIndex = PlayerPrefs.GetInt("SelectedRecordingIndex", -1);
            if (selectedIndex >= 0 && selectedIndex < recordings.Count)
            {
                currentRecording = recordings[selectedIndex];
                StartCoroutine(Playback());
            }
        }
    }

    void LoadDropdownOptions()
    {
        recordingsDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (Recording rec in recordings)
        {
            options.Add(rec.timestamp);
        }
        recordingsDropdown.AddOptions(options);
    }

    [System.Serializable]
    public class Recording
    {
        public List<byte[]> frames;
        public string timestamp;

        public Recording()
        {
            frames = new List<byte[]>();
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public T data;
        public SerializationWrapper(T data) { this.data = data; }
    }
}
