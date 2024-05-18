using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PauseRecordedGameplay : MonoBehaviour
{
    public Sprite pausedImage;
    public Sprite reproductImage;
    float timeScale;

    bool paused = false;

    public void HandlePause()
    {
        paused = !paused;
        if (paused)
        {
            GetComponent<Image>().sprite = pausedImage;
            timeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            GetComponent<Image>().sprite = reproductImage;
            Time.timeScale = timeScale;
        }
    }

    public bool IsPaused()
    {
        return paused;
    }
}
