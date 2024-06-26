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

    public void ShowHighscoreTable(List<GoalAttempt> goalAttempts)
    {
        // Clear previous entries
        foreach (Transform child in entryContainer)
        {
            if (child != entryTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        float templateHeight = 30f;

        for (int i = 0; i < goalAttempts.Count; i++)
        {
            GoalAttempt attempt = goalAttempts[i];

            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);

            int number = attempt.attemptNo;
            string goalPos = attempt.goalPosition.ToString();
            float reflexTime = attempt.reflexTime;
            bool isSaved = attempt.isSaved;
            string bodyArea = attempt.bodyArea;
            float errorDistance = attempt.errorDistance;

            TextMeshProUGUI noText = entryTransform.Find("no").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI goalPosText = entryTransform.Find("goalPos").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI reflexTimeText = entryTransform.Find("reflexTime").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI isSavedText = entryTransform.Find("isSaved").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI errDistText = entryTransform.Find("errDist").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI bodyAreaText = entryTransform.Find("bodyArea").GetComponent<TextMeshProUGUI>();

            noText.text = number.ToString();
            goalPosText.text = goalPos;
            reflexTimeText.text = reflexTime.ToString("F2");
            isSavedText.text = isSaved ? "Yes" : "No";
            errDistText.text = errorDistance.ToString("F2");
            bodyAreaText.text = bodyArea;
        }
    }
}
