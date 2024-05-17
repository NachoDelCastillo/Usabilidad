using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using LLlibs.ZeroDepJson;

public class Tracker : MonoBehaviour
{
    #region Singleton
    private static Tracker instance;
    public static Tracker Instance => instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion // endregion Singleton

    #region Properties
    ISerializer serializerObject;
    IPersistence persistenceObject;
    List<ITrackerAsset> activeTrackers;

    [Serializable]
    enum SerializerType {
        JSON, CSV
    }
	[Serializable]
	enum PersistenceType {
        LOCAL, SERVER
    }

	[Header("Configuration")]
	[SerializeField] SerializerType serializerType;
	[SerializeField] PersistenceType persistenceType;
    [SerializeField] bool persistImmediately;

    [Header("Active Trackers")]
    [SerializeField] bool generalTracker;
    [SerializeField] bool fishMovementTracker;
	[SerializeField] bool recordTracker;

    int IndexOfTheGameToReproduce = 0;
    Queue<Queue<TrackerEvent>> gamesQueue;
    Queue<Tuple<double, double>> timesStartAndEnd;
    private const int INVALID = -1;

    double timeStart, timeEnd;

    [Serializable]
    public class EventBase
    {
        public string gameVersion;
        public string userID;
        public string eventType;
        public double timeStamp;
        public double localTimeStamp;
        public int platformId = INVALID;
        public int moveDirection = INVALID;
        public bool gameCompleted = false;
        public float mousePosX;
        public float mousePosY;
        public float playerPosX;
        public float playerPosY;
    }

    bool replayMode;
	//El modo de replay deshabilita que se manden eventos
	public bool ReplayMode { 
        get {
            return replayMode;
        }
        set {
            if (replayMode == value) {
                return;
            }

            replayMode = value;

            if (replayMode) {
				persistenceObject.Close();
            }
            else {
                persistenceObject.Open();
            }
        }
    }
    #endregion // endregion Properties

    #region Methods
    public void Init()
    {
        gamesQueue = new Queue<Queue<TrackerEvent>>();
        timesStartAndEnd = new Queue<Tuple<double, double>>();
        ReadFile();

        serializerObject = serializerType switch {
            SerializerType.JSON => new JSONSerializer(),
            SerializerType.CSV => new CSVSerializer(),
            _ => throw new NotImplementedException()
        };

        persistenceObject = persistenceType switch {
			PersistenceType.LOCAL => new FilePersistence(serializerObject),
			PersistenceType.SERVER => new ServerPersistence(),
			_ => throw new NotImplementedException()
		};

        activeTrackers = new List<ITrackerAsset>();
        if (generalTracker) {
            activeTrackers.Add(new GeneralTracker());
        }
        if (fishMovementTracker) {
            activeTrackers.Add(new FishMovementTracker());
        }
        if (recordTracker) {
            activeTrackers.Add(new RecordGameTrackerAsset());
        }

        ReplayMode = false;
    }

	private void Start() {
		TrackEvent(new SessionStartEvent());
	}

	private void OnDestroy()
    {
        if (persistenceObject == null) {
            return;
        }
            
        TrackEvent(new SessionEndEvent());

        FlushEvents();

        CloseFile();
	}

    public void TrackEvent(TrackerEvent trackerEvent)
    {
        if (ReplayMode) {
            return;
        }

        foreach (ITrackerAsset tracker in activeTrackers) {
            if (tracker.accept(trackerEvent)) {
                persistenceObject.Send(trackerEvent, persistImmediately);
                return;
            }
        }
    }

    public void FlushEvents() {
		persistenceObject.Flush();
	}

    public void SetIndexOfTheGameToReproduce(int index)
    {
        IndexOfTheGameToReproduce = index;
    }

    public Queue<TrackerEvent> GetTheGameToReproduce()
    {
        if (IndexOfTheGameToReproduce >= 0 && IndexOfTheGameToReproduce < gamesQueue.Count)
        {
            return gamesQueue.ElementAt(IndexOfTheGameToReproduce);
        }
        return null;
    }

    public Tuple<double,double> GetTimesStartAndEnd()
    {
        if (IndexOfTheGameToReproduce >= 0 && IndexOfTheGameToReproduce < gamesQueue.Count)
        {
            return timesStartAndEnd.ElementAt(IndexOfTheGameToReproduce);
        }
        return null;
    }

    public int GetGameCount()
    {
        return gamesQueue.Count;
    }

    public double GetTimeStamp()
    {
        return gamesQueue.ElementAt(IndexOfTheGameToReproduce).ElementAt(0).getTimeStamp();
    }

    void ReadFile()
    {
        string directoryPath = Application.persistentDataPath;

        if (Directory.Exists(directoryPath))
        {
            // Lista de todos los archivos en el directorio persistente
            string[] files = Directory.GetFiles(directoryPath);

            // Iterar cada archivo en el directorio
            foreach (string file in files)
            {
                // Nombre del archivo
                string fileName = Path.GetFileName(file);

                Queue<TrackerEvent> eventsQueue = new Queue<TrackerEvent>();

                // Ver si es un json
                if (fileName.EndsWith(".json"))
                {
                    // Lee el contenido del archivo JSON como una cadena
                    string jsonContent = File.ReadAllText(file);

                    EventBase[] events = ZVJson.FromJson<EventBase>(jsonContent, true);

                    bool gameStarted = false;

                    foreach (EventBase event_ in events)
                    {
                        string gameVersion = event_.gameVersion;
                        string userID = event_.userID;
                        double timeStamp = event_.timeStamp;
                        double localTimeStamp = event_.localTimeStamp;

                        TrackerEvent trackerEvent = null;

                        switch (event_.eventType)
                        {
                            case "GAME_START":
                                timeStart = localTimeStamp;
                                gameStarted = true;
                                break;
                            case "GAME_END":
                                timeEnd = localTimeStamp;
                                trackerEvent = new GameEndEvent(event_.gameCompleted,gameVersion,
                                    userID,timeStamp,localTimeStamp);
                                
                               Debug.Log("TimeEnd : " + timeEnd);
                                break;
                            case "JUMP_START":
                                trackerEvent = new JumpStartEvent(gameVersion, userID, event_.platformId,
                                    new Vector2(event_.mousePosX, event_.mousePosY),
                                    new Vector2(event_.playerPosX, event_.playerPosY), timeStamp, localTimeStamp);
                                break;
                            case "JUMP_END":
                                trackerEvent = new JumpEndEvent(gameVersion, userID, event_.platformId,
                                    new Vector2(event_.playerPosX, event_.playerPosY), timeStamp, localTimeStamp);
                                break;
                            case "MOVE_START":
                                trackerEvent = new MoveStartEvent(gameVersion, userID,
                                    (MoveStartEvent.MoveDirection)event_.moveDirection, timeStamp, localTimeStamp);
                                break;
                            case "MOVE_END":
                                trackerEvent = new MoveEndEvent(gameVersion, userID, timeStamp, localTimeStamp);
                                break;
                            default:
                                continue;
                        }

                        if (trackerEvent != null && gameStarted)
                            eventsQueue.Enqueue(trackerEvent);

                        if (event_.eventType == "GAME_END")
                        {
                            gameStarted = false;
                            timesStartAndEnd.Enqueue(new Tuple<double, double>(timeStart, timeEnd));
                            gamesQueue.Enqueue(eventsQueue);
                            eventsQueue = new Queue<TrackerEvent>();
                        }

                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("El archivo json no se encontró");
        }


    }

    private void CloseFile()
    {
        persistenceObject.Close();
    }
    #endregion // endregion Methods
}
