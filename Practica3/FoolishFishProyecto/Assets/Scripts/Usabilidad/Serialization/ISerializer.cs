



public interface ISerializer
{


    public abstract string Serialize(TrackerEvent trackerEvent);
    public abstract string GetPrefix();
    public abstract string GetSufix();
    public abstract string GetInterfix();

    // public abstract void Deserialize(); ?

    protected:
    // Con o sin abstract antes de string?
    // string prefix = "";
	// string interfix = "";
	// string sufix = "";
}
