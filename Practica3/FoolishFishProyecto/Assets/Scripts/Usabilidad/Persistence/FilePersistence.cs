using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class FilePersistence : MonoBehaviour, IPersistence
{
    private string eventsLogPath;
    private Queue<string> eventQueue = new Queue<string>();

    void Start()
    {
        // Ruta relativa a la carpeta Logs 
        eventsLogPath = Path.Combine(Application.dataPath, "Scripts", "Usabilidad", "Persistence", "Logs");

        if (!Directory.Exists(eventsLogPath))
        {
            Directory.CreateDirectory(eventsLogPath);
        }
    }

    public void Send(TrackerEvent trackerEvent, ISerializer serializerObject)
    {

        string serializedEvent = serializerObject.Serialize(trackerEvent);

        eventQueue.Enqueue(serializedEvent);

        Queue<string> auxEventQueue = new Queue<string>(eventQueue);

        List<string> eventosSerializados = new List<string>();

        while (auxEventQueue.Count > 0)
        {
            eventosSerializados.Add(auxEventQueue.Dequeue());
        }

        string fileFormat = serializerObject.getFormat();
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "events" + fileFormat);

        if (fileFormat.Equals(".json"))
        {
            string eventsJSON = "[" + string.Join(",\n", eventosSerializados.ToArray()) + "]";
            File.WriteAllText(filePath, eventsJSON);
        }

        else
        {
            string columns = "gameVersion,userID,eventType,timeStamp,arg1";

            string events = columns + string.Join(",", eventosSerializados.ToArray());

            File.WriteAllText(filePath, events);
        }

        Debug.Log("Evento encolado para persistencia: " + serializedEvent); // Mensaje de depuración
    }

    public void Flush()
    {
        eventsLogPath = Path.Combine(Application.dataPath, "Scripts", "Usabilidad", "Persistence", "Logs");
        string filePath = Path.Combine(eventsLogPath, "events.log");
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            while (eventQueue.Count > 0)
            {
                string serializedEvent = eventQueue.Dequeue();
                writer.WriteLine(serializedEvent);
                Debug.Log("Evento persistido en el archivo: " + serializedEvent); // Mensaje de depuración
            }
        }
    }
}
