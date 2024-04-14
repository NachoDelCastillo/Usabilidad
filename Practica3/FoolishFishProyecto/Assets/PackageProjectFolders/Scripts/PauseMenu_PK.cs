using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_PK : MenuManager_PK
{
    AllMenuManager_PK allMenuManager;

    GameObject pauseObj;

    [HideInInspector] static public bool paused = false;

    protected override void ExtraAwake()
    {
        pauseObj = transform.GetChild(0).gameObject;
    }

    protected override void buttonPressed(int index)
    {
        if (!paused) return;

        if (index == 0) Unpause();
        if (index == 1)
        {
			Tracker.Instance.TrackEvent(new GameEndEvent(false));
            Tracker.Instance.FlushEvents();

			Unpause();
            GameManager.GetInstance().ChangeScene("Gameplay");

			Tracker.Instance.TrackEvent(new GameStartEvent());
        }
        if (index == 2) {
			Tracker.Instance.TrackEvent(new GameEndEvent(false));
			Tracker.Instance.FlushEvents();

			Unpause(); 
            GameManager.GetInstance().ChangeScene("MainMenu_Scene"); 
        }
    }

    protected override void ExtraUpdate()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (paused) Unpause();
            else Pause();
        }
    }

    void Pause()
    {
        // Sound
        //AudioManager_PK.GetInstance().Play("Pause", 1);

        Time.timeScale = 0;
        paused = true;
        canUseControllerSelection = true;
        pauseObj.SetActive(true);
    }

    void Unpause()
    {
        // Sound
        //AudioManager_PK.GetInstance().Play("Pause", 1);

        Time.timeScale = 1;
        paused = false;
        //StopAllCoroutines();
        canUseControllerSelection = false;
        pauseObj.SetActive(false);
    }
}
