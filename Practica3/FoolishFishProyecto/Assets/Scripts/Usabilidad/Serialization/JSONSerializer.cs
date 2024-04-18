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
}
