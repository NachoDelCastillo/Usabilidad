public interface IPersistence
{
    public void Send(TrackerEvent serializedEvent, ISerializer serializerObject);
    public void Flush();
}
