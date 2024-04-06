public class JumpEndEvent : TrackerEvent
{
    int platformId;

    public JumpEndEvent(int platformId) : base(EventType.JUMP_END)
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
