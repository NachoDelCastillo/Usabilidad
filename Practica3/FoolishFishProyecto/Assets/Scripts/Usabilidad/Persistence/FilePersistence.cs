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

        string fileFormat = serializerObject.getFormat();
        string filePath = Application.persistentDataPath + "/events" + fileFormat;


        if (!File.Exists(filePath))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(filePath))
            {
                if (fileFormat.Equals(".json"))
                {
                    string eventsJSON = serializedEvent + ",\n";
                    sw.Write(eventsJSON);
                }

                else
                {
                    string columns = "gameVersion,userID,eventType,timeStamp,arg1";

                    string events = columns + string.Join(string.Empty, serializedEvent);


                    sw.Write(events);
                }
            }
        }
        else
        {
            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(filePath))
            {
                if (fileFormat.Equals(".json"))
                {
                    string eventsJSON = serializedEvent + ",\n";
                    sw.Write(eventsJSON);
                }

                else
                {
                    string events = string.Join(string.Empty, serializedEvent);
                    sw.Write(events);
                }




            }
        }
        Debug.Log("Evento encolado para persistencia: " + serializedEvent); // Mensaje de depuración
    }

    public void Flush()
    {

    }
}
