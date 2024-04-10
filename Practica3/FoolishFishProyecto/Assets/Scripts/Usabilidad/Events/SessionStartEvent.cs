using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SessionStartEvent : TrackerEvent
{
    DateTime startSessionTime;
    public SessionStartEvent() : base(EventType.SESSION_START)
    {
        
    }

    public override string ToCSV()
    {
        return base.ToCSV() + ",";
    }

    public override string ToJSON()
    {
        return base.ToJSON() + ",\n" + string.Format(
            "\t\"startTime\": \"{0}\"\n",
            startSessionTime.ToString("yyyy-MM-dd")); ;
    }
    
    public void SessionStarted()
    {
        startSessionTime = DateTime.Now;
        Tracker.Instance.TrackEvent(this);
    }

}
