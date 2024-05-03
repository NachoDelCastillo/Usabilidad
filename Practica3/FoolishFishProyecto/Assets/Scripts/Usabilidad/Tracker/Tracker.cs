using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    public bool generalTracker;
    public bool fishMovementTracker;
    public bool recordTracker;
    #endregion // endregion Properties

    #region Methods
    public void Init()
    {
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
        if (recordTracker)
        {
            activeTrackers.Add(new RecordGameTrackerAsset());
        }
    }

	private void Start() {
		TrackEvent(new SessionStartEvent());
	}

	private void OnDestroy()
    {
        TrackEvent(new SessionEndEvent());

        FlushEvents();

        CloseFile();
	}

    public void TrackEvent(TrackerEvent trackerEvent)
    {
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

    private void CloseFile()
    {
        persistenceObject.Close();
    }
    #endregion // endregion Methods
}
