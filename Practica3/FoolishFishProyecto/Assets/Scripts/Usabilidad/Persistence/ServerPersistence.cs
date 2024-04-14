public class ServerPersistence : IPersistence
{
    public void Send(TrackerEvent serializedEvent, ISerializer serializerObject, bool persistImmediately)
    {
        throw new System.NotImplementedException();
    }

    public void Flush(ISerializer serializerObject)
    {
        throw new System.NotImplementedException();
    }
}
