public class MoveEndEvent : TrackerEvent
{
    public MoveEndEvent() : base(EventType.MOVE_END)
    {
    }

    public override string ToCSV()
    {
		return base.ToCSV() + ",";
	}
}
