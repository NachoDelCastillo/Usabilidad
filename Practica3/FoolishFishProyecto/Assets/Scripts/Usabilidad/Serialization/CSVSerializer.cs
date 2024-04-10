using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVSerializer : ISerializer
{
    public string Serialize(TrackerEvent trackerEvent)
    {
        return trackerEvent.ToCSV();
    }

    public string getFormat()
    {
        return ".csv";
    }
}
