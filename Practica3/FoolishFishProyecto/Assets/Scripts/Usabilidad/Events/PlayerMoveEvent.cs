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
        return base.ToCSV();
    }

    public override string ToJSON()
    {
        return base.ToJSON() +
            string.Format(
            "\tplatformId: {0}\n",
            platformId);
    }
}
