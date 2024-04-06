public class FilePersistence : IPersistence
{
    public void Send(string serializedEvent)
    {
        // Guardar en memoria

    }

    public void Flush()
    {
        throw new System.NotImplementedException();
    }
}
