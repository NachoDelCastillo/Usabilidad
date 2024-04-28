using System;

public class MoveStartEvent : TrackerEvent
{
    public enum MoveDirection { LEFT, RIGHT, UP, DOWN }

    MoveDirection moveDirection;

    public MoveStartEvent(MoveDirection moveDirection) : base(EventType.MOVE_START)
    {
        this.moveDirection = moveDirection;
    }

    public MoveStartEvent(string gameVersion, string userId, MoveDirection moveDirection, double timeStamp) : base(gameVersion, userId, EventType.MOVE_START, timeStamp)
    {
        this.moveDirection = moveDirection;
    }

    public override string ToCSV()
    {
        return base.ToCSV() + string.Format(",{0}", (int) moveDirection);
    }

    protected override string CompleteParameters()
    {
        return ",\n" + string.Format( "\t\"moveDirection\": {0}\n", (int) moveDirection);
    }

    public MoveDirection getMoveDirection()
    {
        return moveDirection;
    }
}
