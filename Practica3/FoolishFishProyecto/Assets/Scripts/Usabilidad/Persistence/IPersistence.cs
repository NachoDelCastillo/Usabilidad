public interface IPersistence
{
    public void Send(TrackerEvent serializedEvent, bool persistImmediately);
    public void Flush();

    public void SetSerializerObject(ISerializer serializerObject);

    public void Open();
    public void Close();
}
