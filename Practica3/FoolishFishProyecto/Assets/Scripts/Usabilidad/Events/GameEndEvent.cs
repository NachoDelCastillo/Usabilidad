using System;

public class GameEndEvent : TrackerEvent
{
	bool gameCompleted;

	GameEndEvent() : base(EventType.GAME_END) {

	}

    public override string ToCSV()
    {
        return base.ToCSV() + string.Format(",{0}", gameCompleted);
    }

    public override string ToJSON()
    {
         return base.ToJSON() +
			string.Format(
			"\t\"gameCompleted\": {0}\n",
			gameCompleted);
	}
}
