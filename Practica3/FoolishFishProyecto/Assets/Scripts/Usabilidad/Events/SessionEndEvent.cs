public class SessionEndEvent : TrackerEvent
{
    public SessionEndEvent() : base(EventType.SESSION_END)
    {

    }

    public SessionEndEvent(string gameVersion, string userId, double timeStamp) : base(gameVersion, userId, EventType.SESSION_END, timeStamp)
    {
    }

    public override string ToCSV()
    {
        return base.ToCSV() + ",";
    }

}
