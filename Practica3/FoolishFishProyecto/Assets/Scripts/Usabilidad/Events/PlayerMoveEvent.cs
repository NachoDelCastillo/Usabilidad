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
        return base.ToJSON() + ",\n" +
            string.Format(
            "\t\"platformId\": {0}\n",
            platformId);
    }
}
