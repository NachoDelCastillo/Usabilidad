using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PopulateRecordMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject playButtonPrefab;

    [SerializeField]
    private Button_PK exitButton;

    [SerializeField] 
    private GameObject defaultTextGO;

    [SerializeField]
    private GameObject parent;

    void Start()
    {
        int gameCount = Tracker.Instance.GetGameCount();

        if (gameCount > 0 )
        {
            defaultTextGO.SetActive(false);
        }

        int index = 0;
        for (; index < gameCount; index++)
        {
            // temporal set index
            Tracker.Instance.SetIndexOfTheGameToReproduce(index);

            GameObject go = Instantiate(playButtonPrefab, parent.transform);

            Button_PK goButtonPK = go.GetComponent<Button_PK>();

            // Set info
            goButtonPK.SetIndex(index);

            double timestamp = Tracker.Instance.GetTimeStamp();

            DateTime epochStart = TrackerEvent.epochStart;

            string date = epochStart.AddSeconds(timestamp).ToString();
            Tuple<double, double> timesStartAndEnd = Tracker.Instance.GetTimesStartAndEnd();
            double duration = timesStartAndEnd.Item2 - timesStartAndEnd.Item1;

            string buttonText = date + " - " + duration.ToString("0.00") + " seconds";

            goButtonPK.SetText(buttonText);
        }

        // Populate buttons
        exitButton.SetIndex(index);
        GetComponent<RecordedMenu_PK>().PopulateButtons();

        Tracker.Instance.SetIndexOfTheGameToReproduce(0);
    }
}
