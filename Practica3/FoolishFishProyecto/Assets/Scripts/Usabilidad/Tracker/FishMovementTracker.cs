/// <summary>
/// Tracker que recoge los eventos relacionados con el objetivo 1:
/// Comprobar si el jugador es capaz de entender las mecánicas de
/// salto, movimiento y rebote del personaje
/// </summary>
public class FishMovementTracker : ITrackerAsset {
	public bool accept(TrackerEvent trackerEvent) {
		return trackerEvent.Type() switch {
			TrackerEvent.EventType.JUMP_START => true,
			TrackerEvent.EventType.JUMP_END => true,
			TrackerEvent.EventType.MOVE_START => true,
			_ => false
		};
	}
}
