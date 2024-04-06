using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    #region Singleton
    private static Tracker instance;
    public static Tracker Instance { get { return instance; } }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion // endregion Singleton

    #region Properties
    IPersistence persistenceObject;
    List<ITrackerAsset> activeTrackers;
    #endregion // endregion Properties

    #region Methods
    public void Init()
    {

    }

    public void End()
    {

    }
    public void TrackEvent()
    {

    }
    #endregion // endregion Methods
}
