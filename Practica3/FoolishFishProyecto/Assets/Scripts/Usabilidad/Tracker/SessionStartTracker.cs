using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionStartTracker : ITrackerAsset
{
    public bool accept(TrackerEvent trackerEvent)
    {
        return trackerEvent.Type() switch
        {
            TrackerEvent.EventType.SESSION_START => true,
            _ => false
        };
    }
}
