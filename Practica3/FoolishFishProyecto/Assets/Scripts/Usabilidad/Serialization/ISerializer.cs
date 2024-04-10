public interface ISerializer
{
    public abstract string Serialize(TrackerEvent trackerEvent);

    public abstract string getFormat();
}
