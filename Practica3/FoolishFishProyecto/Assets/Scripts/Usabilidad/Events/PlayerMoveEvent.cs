using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveEvent : TrackerEvent
{
    int platformId;

    public PlayerMoveEvent(int platformId) : base(EventType.PLAYER_MOVE)
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
