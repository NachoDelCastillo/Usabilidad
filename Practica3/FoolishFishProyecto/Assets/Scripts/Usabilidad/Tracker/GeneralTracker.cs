/// <summary>
/// Tracker que recoge los eventos generales
/// </summary>
public class GeneralTracker : ITrackerAsset {
	public bool accept(TrackerEvent trackerEvent) {
		return trackerEvent.Type() switch {
			TrackerEvent.EventType.SESSION_START => true,
			TrackerEvent.EventType.SESSION_END => true,
			TrackerEvent.EventType.GAME_START => true,
			TrackerEvent.EventType.GAME_END => true,
			_ => false
		};
	}
}