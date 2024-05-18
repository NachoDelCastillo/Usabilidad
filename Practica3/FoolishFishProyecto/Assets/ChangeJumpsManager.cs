using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeJumpsManager : MonoBehaviour
{
    [SerializeField]
     private InfoRecordered infoRecorderd;
    [SerializeField]
    private PauseRecordedGameplay pauseRecorded;


    public void NextJump()
    {
        infoRecorderd.NextJump();
        if (pauseRecorded.IsPaused())
        {
            pauseRecorded.HandlePause();
        }
    }

    public void PrevJump() { 

        infoRecorderd.PrevJump();
        if (pauseRecorded.IsPaused())
        {
            pauseRecorded.HandlePause();
        }
    }

}
