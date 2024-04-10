public class ServerPersistence : IPersistence
{
    public void Send(TrackerEvent serializedEvent, ISerializer serializerObject)
    {
        throw new System.NotImplementedException();
    }

    public void Flush()
    {
        throw new System.NotImplementedException();
    }
}
