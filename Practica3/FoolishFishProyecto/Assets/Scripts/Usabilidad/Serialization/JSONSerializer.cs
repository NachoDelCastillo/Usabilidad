using System.IO;

using UnityEngine;

public class JSONSerializer : ISerializer
{
    public string Serialize(TrackerEvent trackerEvent)
    {
		return trackerEvent.ToJSON();
	}

	public string getFormat()
	{
		return ".json";
	}

	string ISerializer.Header() {
		return "[";
	}

	string ISerializer.Prefix() {
		return ",\n";
	}

	string ISerializer.Suffix() {
		return string.Empty;
	}

	string ISerializer.EndOfFile() {
		return "]";
	}

	int ISerializer.SeekEndOffset() {
		return -1;
	}
}
