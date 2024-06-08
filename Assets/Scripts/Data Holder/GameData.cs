using System.Collections.Generic;

[System.Serializable]
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

    public void AddRoundData(string goalPosition, int score, float reflectTime, float errorDistance, string bodyArea)
    {
        Rounds.Add(new RoundData
        {
            GoalPosition = goalPosition,
            Score = score,
            ReflectTime = reflectTime,
            ErrorDistance = errorDistance,
            BodyArea = bodyArea
        });
    }
}
