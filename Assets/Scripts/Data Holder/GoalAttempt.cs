using UnityEngine;

[System.Serializable]
public class GoalAttempt
{
    public int attemptNo;
    public bool isSaved;
    public GoalPosition goalPosition; // Enum for goal position
    public float reflexTime;
    public float errorDistance;
    public string bodyArea;

    public GoalAttempt(int attemptNo, bool isSaved, GoalPosition goalPosition, float reflexTime, float errorDistance, string bodyArea)
    {
        this.attemptNo = attemptNo;
        this.isSaved = isSaved;
        this.goalPosition = goalPosition;
        this.reflexTime = reflexTime;
        this.errorDistance = errorDistance;
        this.bodyArea = bodyArea;
    }
}
