public interface IPersistence
{
    public void Send(string serializedEvent);
    public void Flush();
}
