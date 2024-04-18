using System.IO;

public interface ISerializer
{
    public abstract string Serialize(TrackerEvent trackerEvent);

    public abstract string getFormat();
	string Header();
	string Prefix();
	string Suffix();
	string EndOfFile();

	int SeekEndOffset();
}
