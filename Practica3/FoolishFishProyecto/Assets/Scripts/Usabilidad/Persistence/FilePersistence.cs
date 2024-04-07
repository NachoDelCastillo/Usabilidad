using UnityEngine;
using System.Collections.Generic;
using System.IO;

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

    public void Send(string serializedEvent)
    {
        eventQueue.Enqueue(serializedEvent);
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
