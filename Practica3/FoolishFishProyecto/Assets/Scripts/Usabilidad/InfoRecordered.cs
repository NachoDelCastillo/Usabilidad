using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class infoRecordered : MonoBehaviour
{

    Queue<TrackerEvent> eventsQueue;

    [Serializable]
    struct jsonObjectInfo
    {
        string gameVersion;
        string userId;
        string eventType;
        double timeStamp;
        int platformId;
        int moveDirection;
        bool gameCompleted;
    }

    void Start()
    {
        readFile();
    }

    void readFile()
    {
        // Ruta del archivo CSV en Application.persistentDataPath
        string filePath = Path.Combine(Application.persistentDataPath, "events.json");

        // Verifica que el archivo exista antes de intentar leerlo
        if (File.Exists(filePath))
        {
            // Lee el contenido del archivo JSON como una cadena
            string jsonContent = File.ReadAllText(filePath);

        }
        else
        {
            Debug.LogWarning($"El archivo CSV no se encontró en la ruta: {filePath}");
        }
    }

    void Update()
    {
        
    }
}
