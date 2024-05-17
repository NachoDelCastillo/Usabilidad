using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeJumpsManager : MonoBehaviour
{
    [SerializeField]
     private InfoRecordered infoRecorderd;


    public void NextJump()
    {
        infoRecorderd.NextJump();
    }

    public void PrevJump() { 

        infoRecorderd.PrevJump();
    }

}
