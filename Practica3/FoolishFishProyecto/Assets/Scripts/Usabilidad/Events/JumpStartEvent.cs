public class JumpStartEvent : TrackerEvent
{
    int platformId;

    public JumpStartEvent(int platformId): base(EventType.JUMP_START)
    {
        this.platformId = platformId;
    }

    public JumpStartEvent(string gameVersion, string userId, int platformId, double timeStamp) : base(gameVersion, userId, EventType.JUMP_START, timeStamp)
    {
        this.platformId = platformId;
    }

    public override string ToCSV()
    {
        return base.ToCSV() + string.Format(",{0}", platformId);
    }

    protected override string CompleteParameters()
    {
        return ",\n" + string.Format( "\t\"platformId\": {0}\n", platformId);
    }

    public int getPlatformId()
    {
        return platformId;
    }
}
