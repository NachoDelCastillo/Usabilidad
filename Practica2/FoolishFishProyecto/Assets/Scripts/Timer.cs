using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    TMP_Text timerText;

    bool running = false;

    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TMP_Text>();

        timerText.text = "0:00";

        currentTime = 0f;

        //StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!running) return;

        currentTime += Time.deltaTime;

        UpdateTimer();
    }

    public void RestartTimer()
    {
        timerText.text = "0:00";

        currentTime = 0f;
    }

    public void StartTimer()
    {
        running = true;
    }

    public void StopTimer()
    {
        running = false;
    }

    public void UpdateTimer()
    {
        
        timerText.text = GetTimeString(currentTime);
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public static string GetTimeString(float time)
    {
        string final = "";

        int hours = (int)(time / 3600);
        if (hours > 0)
            final += hours.ToString() + ":";

        time -= hours * 3600;
        final += ((int)(time / 60)).ToString() + ":";

        short nToSlice = 6;

        float seconds = (time % 60f);
        if (seconds < 10)
        {
            final += "0";
            nToSlice = 5;
        }
        if (nToSlice < seconds.ToString().Length)
            final += seconds.ToString()[..nToSlice];
        else
        {
            final += seconds.ToString();
            if (nToSlice - 1 == seconds.ToString().Length)
            {
                final += "0";
            }
        }
        return final;
    }

}
