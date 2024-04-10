using UnityEngine;

public class JSONSerializer : ISerializer
{
    public string Serialize(TrackerEvent trackerEvent)
    {
		return "{\n" + 
			trackerEvent.ToJSON() + 
			"}";
	}

	public string getFormat()
	{
		return ".json";
	}
}
