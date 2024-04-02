public interface IPersistence
{
    public abstract void Send(TrackerEvent trackerEvent);
    public abstract void Flush();
}
