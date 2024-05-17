using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoRecordered : MonoBehaviour
{
    // Devuelve true si se esta visualizando una repeticion de una partida
    static public bool playingRecordedGame { private set; get; }

    double timeStart, timeEnd, offset;

    Queue<TrackerEvent> eventsQueue;
    List<double> jumpStartTimes;

    // Referencias
    // Referencia al script de movimiento del personaje principal
    FishMovement fishMovement;
    [SerializeField]
    private Slider progressBar;
    [SerializeField] GameObject markerPrefab;
    [SerializeField] GameObject flyingTimer;
    double currentGameTime;

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

        jumpStartTimes = new List<double>();

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
            currentGameTime = Time.time - offset;
            Debug.Log(currentGameTime);

            // Si el tiempo del pr�ximo evento es menor o igual al tiempo actual del juego
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
        int jumpIndex = 0;
        foreach (TrackerEvent trackerEvent_ in eventsQueue)
        {
            if (trackerEvent_.Type() != TrackerEvent.EventType.JUMP_START)
                continue;

            Vector2 markerPosition = new Vector2((float)
                ((trackerEvent_.getLocalTimeStamp() - timeStart) / timeEnd * 0.96f + 0.011f) * progressBar.GetComponent<RectTransform>().sizeDelta.x, 0);

            GameObject instance = Instantiate(markerPrefab, progressBar.transform);
            instance.GetComponent<RectTransform>().anchoredPosition = markerPosition;
            instance.GetComponent<JumpButton>().jumpButtonIndex = jumpIndex;
            jumpStartTimes.Add(trackerEvent_.getLocalTimeStamp()-timeStart);
            jumpIndex++;
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

    public void ResetEventQueue(int jumpIndex)
    {
        int numJumsps = 0;
        eventsQueue.Clear();
        eventsQueue = new Queue<TrackerEvent>(Tracker.Instance.GetTheGameToReproduce());
        Queue<TrackerEvent> aux = new Queue<TrackerEvent>(Tracker.Instance.GetTheGameToReproduce());
        foreach (TrackerEvent trackerEvent_ in aux)
        {
            if (trackerEvent_.Type() != TrackerEvent.EventType.JUMP_END && jumpIndex!=0)
            {
                eventsQueue.Dequeue();
            }
            else if(trackerEvent_.Type() == TrackerEvent.EventType.JUMP_END)
            {
                if(numJumsps==jumpIndex)
                {
                    JumpEndEvent event_ = (JumpEndEvent) trackerEvent_;
                    fishMovement.transform.position = event_.getPlayerPos();
                    double auxOffset = Time.time - offset - jumpStartTimes[jumpIndex];
                    offset += auxOffset+1;
                    break;
                }
                else
                {
                    eventsQueue.Dequeue();
                    numJumsps++;
                }
            }
        }

    }
}
