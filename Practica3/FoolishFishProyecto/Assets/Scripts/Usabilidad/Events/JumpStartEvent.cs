public class JumpStartEvent : TrackerEvent
{
    int platformId;

    public JumpStartEvent(int platformId): base(EventType.JUMP_START)
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
}
