public interface IPersistence
{
    public void Send(TrackerEvent serializedEvent, ISerializer serializerObject, bool persistImmediately);
    public void Flush(ISerializer serializerObject);
}
