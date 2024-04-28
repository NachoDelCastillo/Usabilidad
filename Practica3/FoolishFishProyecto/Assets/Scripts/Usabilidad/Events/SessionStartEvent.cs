using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SessionStartEvent : TrackerEvent
{
    public SessionStartEvent() : base(EventType.SESSION_START)
    {
        
    }

    public SessionStartEvent(string gameVersion, string userId, double timeStamp) : base(gameVersion, userId, EventType.SESSION_START, timeStamp)
    {
    }

    public override string ToCSV()
    {
        return base.ToCSV() + ",";
    }
}
