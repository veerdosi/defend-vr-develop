using System.Collections.Generic;

public class GameData
{
    public List<SessionData> Sessions { get; private set; } = new List<SessionData>();

    public void AddSessionData(SessionData sessionData)
    {
        Sessions.Add(sessionData);
    }
}

[System.Serializable]
public class SessionData
{
    public List<RoundData> Rounds { get; private set; } = new List<RoundData>();

    public void AddRoundData(string goalPosition, int score, float initiationTime, float errorDistance, string bodyArea)
    {
        Rounds.Add(new RoundData
        {
            GoalPosition = goalPosition,
            Score = score,
            InitiationTime = initiationTime,
            ErrorDistance = errorDistance,
            BodyArea = bodyArea
        });
    }
}
