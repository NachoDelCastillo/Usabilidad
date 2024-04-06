public class GameEndEvent : TrackerEvent
{
	bool gameCompleted;
	GameEndEvent() : base(EventType.GAME_END) {

	}

    public override string ToJSON()
    {
         return base.ToJSON() +
			string.Format(
			"\tgameCompleted: {0}\n",
			gameCompleted);
	}
}
