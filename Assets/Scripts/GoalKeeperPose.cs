using UnityEngine;
using UnityEngine.UI;

public class GoalkeeperPose : MonoBehaviour
{
    public Transform headTracker;
    public Transform leftHandTracker;
    public Transform rightHandTracker;
    public Transform waistTracker;
    public Transform leftKneeTracker;
    public Transform rightKneeTracker;

    public Text feedbackText; // UI Text element for feedback
    public GameObject ghostModel; // Ghost model for visual cue

    public float requiredKneeAngle = 90f; // Example value
    public float requiredWaistAngle = 30f; // Example value
    public float requiredHandDistance = 1.5f; // Example value

    void Update()
    {
        if (IsCorrectPose())
        {
            feedbackText.text = "Correct pose! Proceed to the next part.";
            ghostModel.SetActive(false); // Hide the ghost model
            // Allow the player to continue
        }
        else
        {
            feedbackText.text = "Incorrect pose! Adjust your posture.";
            ghostModel.SetActive(true); // Show the ghost model
        }
    }

    bool IsCorrectPose()
    {
        float kneeAngle = CalculateAngle(leftKneeTracker, waistTracker, rightKneeTracker);
        float waistAngle = CalculateAngle(headTracker, waistTracker, Vector3.down); // Assuming down is forward for bending
        float handDistance = Vector3.Distance(leftHandTracker.position, rightHandTracker.position);

        bool isKneeCorrect = kneeAngle >= requiredKneeAngle - 10f && kneeAngle <= requiredKneeAngle + 10f;
        bool isWaistCorrect = waistAngle >= requiredWaistAngle - 5f && waistAngle <= requiredWaistAngle + 5f;
        bool areHandsCorrect = handDistance >= requiredHandDistance - 0.2f && handDistance <= requiredHandDistance + 0.2f;

        return isKneeCorrect && isWaistCorrect && areHandsCorrect;
    }

    float CalculateAngle(Transform a, Transform b, Transform c)
    {
        Vector3 ab = b.position - a.position;
        Vector3 bc = c.position - b.position;
        return Vector3.Angle(ab, bc);
    }

    float CalculateAngle(Transform a, Transform b, Vector3 direction)
    {
        Vector3 ab = b.position - a.position;
        return Vector3.Angle(ab, direction);
    }
}
