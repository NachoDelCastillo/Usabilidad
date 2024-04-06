public class SessionEndEvent : TrackerEvent
{
    SessionEndEvent() : base(EventType.SESSION_END)
    {

    }
    public override string ToJSON()
    {
        return base.ToJSON();
    }
}
