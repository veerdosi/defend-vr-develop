using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreTable : MonoBehaviour
{
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;

    private void Awake()
    {
        if (entryContainer == null)
        {
            entryContainer = transform.Find("highscoreEntryContainer");
        }

        if (entryTemplate == null)
        {
            entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        }

        if (entryContainer == null || entryTemplate == null)
        {
            Debug.LogError("Entry container or template is missing!");
            return;
        }

        entryTemplate.gameObject.SetActive(false);
    }

    public void ShowHighscoreTable(SessionData sessionData)
    {
        foreach (Transform child in entryContainer)
        {
            if (child != entryTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        float templateHeight = 30f;

        for (int i = 0; i < sessionData.Rounds.Count; i++)
        {
            RoundData round = sessionData.Rounds[i];

            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);

            int number = i + 1;
            string goalPos = round.GoalPosition;
            float refTime = round.ReflectTime;
            int score = round.Score;
            string bodyArea = round.BodyArea;

            TextMeshProUGUI noText = entryTransform.Find("no").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI goalPosText = entryTransform.Find("goalPos").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI refTimeText = entryTransform.Find("refTime").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI scoreText = entryTransform.Find("score").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI errDistText = entryTransform.Find("errDist").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI bodyAreaText = entryTransform.Find("bodyArea").GetComponent<TextMeshProUGUI>();

            noText.text = number.ToString();
            goalPosText.text = goalPos;
            refTimeText.text = refTime.ToString("F2");
            scoreText.text = score.ToString();
            errDistText.text = round.ErrorDistance.ToString("F2");
            bodyAreaText.text = bodyArea;
        }
    }
}
