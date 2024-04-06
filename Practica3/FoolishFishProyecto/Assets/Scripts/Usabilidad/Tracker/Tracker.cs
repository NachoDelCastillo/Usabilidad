using System;
using System.Collections.Generic;
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

    [Header("Active Trackers")]
    [SerializeField] bool generalTracker;
    [SerializeField] bool fishMovementTracker;
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
			PersistenceType.LOCAL => new FilePersistence(),
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
    }

    public void End()
    {
        persistenceObject.Flush();
    }

    public void TrackEvent(TrackerEvent trackerEvent)
    {
        foreach (ITrackerAsset tracker in activeTrackers) {
            if (tracker.accept(trackerEvent)) {
                persistenceObject.Send(serializerObject.Serialize(trackerEvent));
                return;
            }
        }
    }
    #endregion // endregion Methods
}
