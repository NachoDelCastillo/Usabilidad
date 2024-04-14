using System;

public class GameEndEvent : TrackerEvent
{
    bool gameCompleted;

    public GameEndEvent(bool gameCompleted) : base(EventType.GAME_END)
    {
        this.gameCompleted = gameCompleted;
    }

    public override string ToCSV()
    {
        return base.ToCSV() + string.Format(",{0}", gameCompleted);
    }

    public override string ToJSON()
    {
        return base.ToJSON() + ",\n" +
                    string.Format("\t\"gameCompleted\": {0}\n", gameCompleted.ToString().ToLower());
    }
}
