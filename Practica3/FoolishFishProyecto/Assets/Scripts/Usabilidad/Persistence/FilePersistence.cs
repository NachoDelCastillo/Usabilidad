using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilePersistence : IPersistence
{
    public void Send(TrackerEvent trackerEvent)
    {
        // Serializar el evento
        ISerializer serializer = new JSONSerializer();

        string serializedEvent = serializer.Serialize(trackerEvent);

        // Guardar en memoria

    }

    public void Flush()
    {
        throw new System.NotImplementedException();
    }
}
