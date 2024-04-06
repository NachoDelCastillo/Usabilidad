public class JumpStartEvent : TrackerEvent
{
    int platformId;

    public JumpStartEvent(int platformId): base(EventType.JUMP_START)
    {
        this.platformId = platformId;
    }

	public override string ToJSON() {
		return base.ToJSON() +
			string.Format(
			"\tplatformId: {0}\n",
			platformId);
	}
}