/// <summary>
/// Tracker que recoge los eventos generales
/// </summary>
public class RecordGameTrackerAsset : ITrackerAsset {
	public bool accept(TrackerEvent trackerEvent) {
		return trackerEvent.Type() switch {
			TrackerEvent.EventType.GAME_START => true,
			TrackerEvent.EventType.GAME_END => true,
			TrackerEvent.EventType.JUMP_START => true,
			TrackerEvent.EventType.JUMP_END => true,
			TrackerEvent.EventType.MOVE_START => true,
			TrackerEvent.EventType.MOVE_END => true,
			_ => false
		};
	}
}
