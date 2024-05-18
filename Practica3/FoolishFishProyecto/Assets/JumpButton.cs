using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public int jumpButtonIndex;
    public int eventIndex;

    private PauseRecordedGameplay pauseRecorded;
    private InfoRecordered infoRecordered;

    private void Start()
    {
        pauseRecorded = GameObject.Find("PauseButton").GetComponent<PauseRecordedGameplay>();
        infoRecordered = GameObject.Find("ReplayGame").GetComponent<InfoRecordered>();
    }

    public void JumpButtonClicked()
    {
        
        infoRecordered.ResetEventQueue(jumpButtonIndex,eventIndex);

        if (pauseRecorded.IsPaused())
        {
            pauseRecorded.HandlePause();
        }
    }
}
