using LLlibs.ZeroDepJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.EventSystems;

public class infoRecordered : MonoBehaviour
{

    Queue<TrackerEvent> eventsQueue;
    private const int INVALID = -1;

    [Serializable]
    public class EventBase
    {
        public string gameVersion;
        public string userID;
        public string eventType;
        public double timeStamp;
        public int platformId = INVALID;
        public int moveDirection = INVALID;
        public bool gameCompleted = false;
        public float mousePosX;
        public float mousePosY;
    }

    void Start()
    {
        eventsQueue = new Queue<TrackerEvent>();
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

            EventBase[] events = ZVJson.FromJson<EventBase>(jsonContent, true);

            foreach (EventBase event_ in events)
            {

                string gameVersion = event_.gameVersion;
                string userID = event_.userID;
                double timeStamp = event_.timeStamp;

                TrackerEvent trackerEvent = null;

                switch (event_.eventType)
                {
                    case "SESSION_START":
                        trackerEvent = new SessionStartEvent(gameVersion, userID, timeStamp);
                        break;
                    case "SESSION_END":
                        trackerEvent = new SessionEndEvent(gameVersion, userID, timeStamp);
                        break;
                    case "GAME_START":
                        trackerEvent = new GameStartEvent(gameVersion, userID, timeStamp);
                        break;
                    case "GAME_END":
                        trackerEvent = new GameEndEvent(gameVersion, userID, event_.gameCompleted ,timeStamp);
                        break;
                    case "JUMP_START":
                        trackerEvent = new JumpStartEvent(gameVersion, userID, event_.platformId, 
                            new Vector2(event_.mousePosX, event_.mousePosY), timeStamp);
                        break;
                    case "JUMP_END":
                        trackerEvent = new JumpEndEvent(gameVersion, userID, event_.platformId, timeStamp);
                        break;
                    case "MOVE_START":
                        trackerEvent = new MoveStartEvent(gameVersion, userID, (MoveStartEvent.MoveDirection)event_.moveDirection, timeStamp);
                        break;
                    case "MOVE_END":
                        trackerEvent = new MoveEndEvent(gameVersion, userID, timeStamp);
                        break;

                }

                eventsQueue.Enqueue(trackerEvent);
            }

            int i = 0;
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
