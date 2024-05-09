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
    // Devuelve true si se esta visualizando una repeticion de una partida
    static public bool playingRecordedGame { private set; get; }

    Queue<TrackerEvent> eventsQueue;
    private const int INVALID = -1;

    double timeStart, timeEnd, offset;

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
        public float playerPosX;
        public float playerPosY;
    }

    // Referencias
    // Referencia al script de movimiento del personaje principal
    FishMovement fishMovement;
    [SerializeField]
    private GameObject progressBar;

    void Start()
    {
        // Debug
        playingRecordedGame = true;
        fishMovement = FindAnyObjectByType<FishMovement>();

        eventsQueue = new Queue<TrackerEvent>();
        readFile();

        progressBar.SetActive(true);
        progressBar.GetComponentInChildren<ProgressBar>().MoveBar((float)(timeEnd-timeStart));

        offset = Time.time;
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

            double timeStampGameStart = 0;

            foreach (EventBase event_ in events)
            {

                string gameVersion = event_.gameVersion;
                string userID = event_.userID;
                double timeStamp = event_.timeStamp - timeStampGameStart;

                TrackerEvent trackerEvent = null;

                switch (event_.eventType)
                {
                    case "SESSION_START":
                        trackerEvent = new SessionStartEvent(gameVersion, userID, 0);
                        timeStart = event_.timeStamp;
                        Debug.Log("TimeStart : " + timeStart);
                        break;
                    case "SESSION_END":
                        trackerEvent = new SessionEndEvent(gameVersion, userID, timeStamp);
                        break;
                    case "GAME_START":
                        trackerEvent = new GameStartEvent(gameVersion, userID, 0);
                        timeStampGameStart = timeStamp;
                        break;
                    case "GAME_END":
                        trackerEvent = new GameEndEvent(gameVersion, userID, event_.gameCompleted, timeStamp);
                        timeEnd = event_.timeStamp;
                        Debug.Log("TimeEnd : " + timeEnd);
                        break;
                    case "JUMP_START":
                        trackerEvent = new JumpStartEvent(gameVersion, userID, event_.platformId,
                            new Vector2(event_.mousePosX, event_.mousePosY), new Vector2(event_.playerPosX, event_.playerPosY), timeStamp);
                        break;
                    case "JUMP_END":
                        trackerEvent = new JumpEndEvent(gameVersion, userID, event_.platformId, new Vector2(event_.playerPosX, event_.playerPosY), timeStamp);
                        break;
                    case "MOVE_START":
                        trackerEvent = new MoveStartEvent(gameVersion, userID, (MoveStartEvent.MoveDirection)event_.moveDirection, timeStamp);
                        break;
                    case "MOVE_END":
                        trackerEvent = new MoveEndEvent(gameVersion, userID, timeStamp);
                        break;

                }

                eventsQueue.Enqueue(trackerEvent);

                if (event_.eventType == "GAME_END") break;
            }
        }
        else
        {
            Debug.LogWarning($"El archivo CSV no se encontró en la ruta: {filePath}");
        }
    }

    void Update()
    {
        if (eventsQueue.Count > 0)
        {
            // Obtener el primer evento de la cola sin quitarlo
            TrackerEvent nextEvent = eventsQueue.Peek(); 

            // Obtener el tiempo actual del juego   
            double currentGameTime = Time.time - offset;

            // Si el tiempo del próximo evento es menor o igual al tiempo actual del juego
            if (nextEvent.getTimeStamp() <= currentGameTime)
            {
                // Procesar el evento y quitarlo de la cola
                ProcessEvent(eventsQueue.Dequeue()); 
            }
        }
    }

    void ProcessEvent(TrackerEvent trackerEvent)
    {
        switch (trackerEvent.GetEventTypeString())
        {
            case "GAME_END":
                Debug.Log("RecordedEvent : GameEnd");
                GameManager.GetInstance().ChangeScene("ReplayMenu");
                break;

            case "JUMP_START":

                Debug.Log("RecordedEvent : JumpStart");
                JumpStartEvent jumpStartEvent = (JumpStartEvent)trackerEvent;
                fishMovement.Process_JumpStartEvent(jumpStartEvent.getPlayerPos(), jumpStartEvent.getMousePos());
                break;

            case "JUMP_END":

                Debug.Log("RecordedEvent : JumpEnd");
                JumpEndEvent jumpEndEvent = (JumpEndEvent)trackerEvent;
                fishMovement.Process_JumpEndEvent(jumpEndEvent.getPlayerPos());
                break;

            case "MOVE_START":
                Debug.Log("RecordedEvent : MoveStart");
                MoveStartEvent moveStartEvent = (MoveStartEvent)trackerEvent;
                MoveStartEvent.MoveDirection moveDirection = moveStartEvent.getMoveDirection();
                fishMovement.Process_MoveStartEvent(moveDirection);
                break;

            case "MOVE_END":
                Debug.Log("RecordedEvent : MoveEnd");
                fishMovement.Process_MoveEndEvent();
                break;
        }
    }
}
