public class SessionStartEvent : TrackerEvent
{
    public SessionStartEvent() : base(EventType.SESSION_START)
    {
        
    }
    public override string ToJSON()
    {
        return base.ToJSON();
    }
}
