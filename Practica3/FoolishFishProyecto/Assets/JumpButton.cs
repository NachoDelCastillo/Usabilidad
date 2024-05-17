using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public int jumpButtonIndex;
    public int eventIndex;

    public void JumpButtonClicked()
    {
        InfoRecordered infoRecordered = GameObject.Find("ReplayGame").GetComponent<InfoRecordered>();
        infoRecordered.ResetEventQueue(jumpButtonIndex,eventIndex);
    }
}
