public class MoveEndEvent : TrackerEvent
{
    public MoveEndEvent() : base(EventType.MOVE_END)
    {
    }

    public MoveEndEvent(string gameVersion, string userId, double timeStamp, double localTimeStamp) 
        : base(gameVersion, userId, EventType.MOVE_END, timeStamp, localTimeStamp)
    {
    }

    public override string ToCSV()
    {
		return base.ToCSV() + ",";
	}
}
