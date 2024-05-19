using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_PK : MenuManager_PK
{
    AllMenuManager_PK allMenuManager;

    GameObject pauseObj;
    [SerializeField]
    private GameObject progressBarUI;
    [SerializeField]
    private GameObject pauseButton;

    [HideInInspector] static public bool paused = false;

    float timeScale;

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
			if (Tracker.Instance != null)
                Tracker.Instance.TrackEvent(new GameEndEvent(false));
            Tracker.Instance.FlushEvents();

			Unpause();
            GameManager.GetInstance().ChangeScene("Gameplay");

			if (Tracker.Instance != null)
                Tracker.Instance.TrackEvent(new GameStartEvent());
        }
        if (index == 2) {
			if (Tracker.Instance != null)
                Tracker.Instance.TrackEvent(new GameEndEvent(false));
            if (Tracker.Instance != null)
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
        timeScale = Time.timeScale;
        Time.timeScale = 0;
        paused = true;
        canUseControllerSelection = true;
        pauseObj.SetActive(true);
        progressBarUI.SetActive(false);
    }

    void Unpause()
    {
        // Sound

        PauseRecordedGameplay pauseRecorededButton;
        if (pauseButton != null)
        {

            pauseRecorededButton = pauseButton.GetComponent<PauseRecordedGameplay>();
            if (pauseRecorededButton == null || (pauseRecorededButton && !pauseRecorededButton.IsPaused()))
            {
                Time.timeScale = timeScale;
            }
        }
        InfoRecordered infoRecordered = GameObject.Find("ReplayGame").GetComponent<InfoRecordered>();
        if (infoRecordered != null)
        {
            progressBarUI.SetActive(true);
        }

        paused = false;
        //StopAllCoroutines();
        canUseControllerSelection = false;
        pauseObj.SetActive(false);
    }
}
