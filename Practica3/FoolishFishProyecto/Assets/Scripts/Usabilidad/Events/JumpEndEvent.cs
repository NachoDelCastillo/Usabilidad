public class JumpEndEvent : TrackerEvent
{
    int platformId;

    public JumpEndEvent(int platformId) : base(EventType.JUMP_END)
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