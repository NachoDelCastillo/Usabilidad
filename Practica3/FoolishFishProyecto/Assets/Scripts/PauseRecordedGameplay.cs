using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseRecordedGameplay : MonoBehaviour
{
    public Sprite pausedImage;
    public Sprite reproductImage;

    bool paused = false;

    public void HandlePause()
    {
        paused = !paused;
        if (paused)
        {
            GetComponent<Image>().sprite = pausedImage;
            Time.timeScale = 0;
        }
        else
        {
            GetComponent<Image>().sprite = reproductImage;
            Time.timeScale = 1;
        }
    }
}
