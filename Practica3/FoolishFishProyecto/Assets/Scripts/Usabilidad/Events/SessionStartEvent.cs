using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SessionStartEvent : TrackerEvent
{
    public SessionStartEvent() : base(EventType.SESSION_START)
    {
        
    }

    public override string ToCSV()
    {
        return base.ToCSV() + ",";
    }
}
