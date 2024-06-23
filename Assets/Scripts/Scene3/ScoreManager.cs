using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public static int score = 0;
    private void Start()
    {

    }
    void Update()
    {
        scoreText.text = "Score: " + Mathf.Round(score);
    }
}
