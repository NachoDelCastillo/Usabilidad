public class GameStartEvent : TrackerEvent
{
    public GameStartEvent() : base(EventType.GAME_START)
    {

    }

    public override string ToJSON()
    {
        return base.ToJSON();      
    }
}
