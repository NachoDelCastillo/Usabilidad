using System.Collections;
using System.Collections.Generic;
using System.IO;

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

	string ISerializer.Header() {
		return "gameVersion,userID,eventType,timeStamp,arg1";
	}

	string ISerializer.Prefix(bool firstEvent) {
		return string.Empty;
	}

	string ISerializer.Suffix() {
		return string.Empty;
	}

	string ISerializer.EndOfFile() {
		return string.Empty;
	}

	int ISerializer.SeekEndOffset() {
		return 0;
	}
}
