using System;

public class GameStartEvent : TrackerEvent
{
    public GameStartEvent() : base(EventType.GAME_START)
    {
    }

    public GameStartEvent(string gameVersion, string userId, double timeStamp) : base(gameVersion, userId, EventType.GAME_START, timeStamp)
    {
    }

    public override string ToCSV()
    {
        return base.ToCSV() + ",";
    }
}
