using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoRecordered : MonoBehaviour
{
    // Devuelve true si se esta visualizando una repeticion de una partida
    static public bool playingRecordedGame { private set; get; }

    double timeStart, timeEnd, offset;

    Queue<TrackerEvent> eventsQueue;

    // Referencias
    // Referencia al script de movimiento del personaje principal
    FishMovement fishMovement;
    [SerializeField]
    private Slider progressBar;
    [SerializeField] GameObject markerPrefab;
    [SerializeField] GameObject flyingTimer;

    void Start()
    {
        if (!Tracker.Instance.ReplayMode)
        {
            playingRecordedGame = false;
            gameObject.SetActive(false);

            progressBar.gameObject.SetActive(false);
            return;
        }
        else
        {
            flyingTimer.SetActive(false);

        }

        // Debug
        playingRecordedGame = true;
        fishMovement = FindAnyObjectByType<FishMovement>();

        eventsQueue = new Queue<TrackerEvent>(Tracker.Instance.GetTheGameToReproduce());
        Tuple<double, double> timesStartAndEnd = Tracker.Instance.GetTimesStartAndEnd();

        timeStart = timesStartAndEnd.Item1;
        timeEnd = timesStartAndEnd.Item2;

        setMarkersInProgessBar();

        progressBar.gameObject.SetActive(true);
        progressBar.value = 0;

        offset = Time.time;
    }

    void Update()
    {
        progressBar.value = (float)((Time.time - offset) / timeEnd);

        if (eventsQueue.Count > 0)
        {
            // Obtener el primer evento de la cola sin quitarlo
            TrackerEvent nextEvent = eventsQueue.Peek();

            // Obtener el tiempo actual del juego   
            double currentGameTime = Time.time - offset;

            // Si el tiempo del próximo evento es menor o igual al tiempo actual del juego
            if (nextEvent.getLocalTimeStamp() - timeStart <= currentGameTime)
            {
                // Procesar el evento y quitarlo de la cola
                ProcessEvent(eventsQueue.Dequeue());
            }

            if(eventsQueue.Count <= 0)
            {
                Debug.Log("RecordedEvent : GameEnd");
                GameManager.GetInstance().ChangeScene("ReplayMenu");
            }
        }
    }

    void setMarkersInProgessBar()
    {
        foreach (TrackerEvent trackerEvent_ in eventsQueue)
        {
            if (trackerEvent_.Type() != TrackerEvent.EventType.JUMP_END)
                continue;

            Vector2 markerPosition = new Vector2((float)
                ((trackerEvent_.getLocalTimeStamp() - timeStart) / timeEnd * 0.96f + 0.011f) * progressBar.GetComponent<RectTransform>().sizeDelta.x, 0);

            Instantiate(markerPrefab, progressBar.transform)
                .GetComponent<RectTransform>().anchoredPosition = markerPosition;
        }
    }

    void ProcessEvent(TrackerEvent trackerEvent)
    {
        switch (trackerEvent.GetEventTypeString())
        {
            case "JUMP_START":

                Debug.Log("RecordedEvent : JumpStart");
                JumpStartEvent jumpStartEvent = (JumpStartEvent)trackerEvent;
                fishMovement.Process_JumpStartEvent(jumpStartEvent.getPlayerPos(), jumpStartEvent.getMousePos());
                break;

            case "JUMP_END":

                Debug.Log("RecordedEvent : JumpEnd");
                JumpEndEvent jumpEndEvent = (JumpEndEvent)trackerEvent;
                fishMovement.Process_JumpEndEvent(jumpEndEvent.getPlayerPos());
                break;

            case "MOVE_START":
                Debug.Log("RecordedEvent : MoveStart");
                MoveStartEvent moveStartEvent = (MoveStartEvent)trackerEvent;
                MoveStartEvent.MoveDirection moveDirection = moveStartEvent.getMoveDirection();
                fishMovement.Process_MoveStartEvent(moveDirection);
                break;

            case "MOVE_END":
                Debug.Log("RecordedEvent : MoveEnd");
                fishMovement.Process_MoveEndEvent();
                break;
        }
    }
}
