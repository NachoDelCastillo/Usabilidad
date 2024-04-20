public class ServerPersistence : IPersistence
{
    public void Send(TrackerEvent serializedEvent, bool persistImmediately)
    {
        throw new System.NotImplementedException();
    }

    public void Flush()
    {
        throw new System.NotImplementedException();
    }

	void IPersistence.SetSerializerObject(ISerializer serializerObject) {
		throw new System.NotImplementedException();
	}

    public void Close()
    {
        throw new System.NotImplementedException();
    }
}
