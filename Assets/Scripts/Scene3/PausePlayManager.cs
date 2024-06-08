using UnityEngine;
using UnityEngine.UI;

public class PausePlayManager : MonoBehaviour
{
    public BallManager ballManager;
    public Text buttonText;
    private bool isPaused = false;
    public void TogglePausePlay()
    {
        isPaused = !isPaused;
        ballManager.TogglePausePlay();
        buttonText.text = isPaused ? "Play" : "Pause";
    }
}
