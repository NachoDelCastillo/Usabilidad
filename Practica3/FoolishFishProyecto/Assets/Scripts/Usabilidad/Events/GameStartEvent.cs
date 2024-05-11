using System;

public class GameStartEvent : TrackerEvent
{
    public GameStartEvent() : base(EventType.GAME_START)
    {
    }

    public override string ToCSV()
    {
        return base.ToCSV() + ",";
    }
}
