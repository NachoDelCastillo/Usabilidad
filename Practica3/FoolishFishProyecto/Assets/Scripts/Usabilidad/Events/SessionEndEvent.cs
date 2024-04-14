public class SessionEndEvent : TrackerEvent
{
    public SessionEndEvent() : base(EventType.SESSION_END)
    {

    }

    public override string ToCSV()
    {
        return base.ToCSV() + ",";
    }

    public override string ToJSON()
    {
        return base.ToJSON() + "\n";
    }
}
