using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoRecordered : MonoBehaviour
{
    // Devuelve true si se esta visualizando una repeticion de una partida
    static public bool playingRecordedGame { private set; get; }

    double timeStart, timeEnd, offset;

    List<TrackerEvent> eventsQueue;
    List<double> jumpEndTimes;
    List<int> jumpEndIndex;

    // Referencias
    // Referencia al script de movimiento del personaje principal
    FishMovement fishMovement;
    [SerializeField]
    private Slider progressBar;
    [SerializeField] GameObject markerPrefab;
    [SerializeField] GameObject flyingTimer;
    double currentGameTime;
    int indexEvent = 0;
    int jumpIndex_ = 0;

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

        eventsQueue = new List<TrackerEvent>(Tracker.Instance.GetTheGameToReproduce());

        Tuple<double, double> timesStartAndEnd = Tracker.Instance.GetTimesStartAndEnd();

        timeStart = timesStartAndEnd.Item1;
        timeEnd = timesStartAndEnd.Item2;

        jumpEndTimes = new List<double>();
        jumpEndIndex = new List<int>();

        setMarkersInProgessBar();

        progressBar.gameObject.SetActive(true);
        progressBar.value = 0;

        offset = Time.time;
    }

    void Update()
    {
        progressBar.value = (float)((Time.time - offset) / timeEnd);

        if (indexEvent < eventsQueue.Count)
        {

            // Obtener el tiempo actual del juego   
            currentGameTime = Time.time - offset;
            //Debug.Log(currentGameTime);

            // Si el tiempo del próximo evento es menor o igual al tiempo actual del juego
            if (eventsQueue[indexEvent].getLocalTimeStamp() - timeStart <= currentGameTime)
            {
                Debug.Log("EVENT WEBOS");

                // Procesar el evento y quitarlo de la cola
                ProcessEvent(eventsQueue[indexEvent]);
                indexEvent++;
            }
        }
        else
        {

            Debug.Log("RecordedEvent : GameEnd");
            eventsQueue.Clear();
            GameManager.GetInstance().ChangeScene("ReplayMenu");
        }
    }

    void setMarkersInProgessBar()
    {
        int indexEvent_ = -1;
        int jumpIndex = 0;
        foreach (TrackerEvent trackerEvent_ in eventsQueue)
        {
            indexEvent_++;
            if (trackerEvent_.Type() != TrackerEvent.EventType.JUMP_END)
                continue;

            Vector2 markerPosition = new Vector2((float)
                ((trackerEvent_.getLocalTimeStamp() - timeStart) / timeEnd * 0.96f + 0.011f) * progressBar.GetComponent<RectTransform>().sizeDelta.x, 0);

            GameObject instance = Instantiate(markerPrefab, progressBar.transform);
            instance.GetComponent<RectTransform>().anchoredPosition = markerPosition;
            instance.GetComponent<JumpButton>().eventIndex = indexEvent_;
            jumpEndIndex.Add(indexEvent_);
            instance.GetComponent<JumpButton>().jumpButtonIndex = jumpIndex;
            jumpEndTimes.Add(trackerEvent_.getLocalTimeStamp()-timeStart);
            jumpIndex++;
        }
    }

    void ProcessEvent(TrackerEvent trackerEvent)
    {
        switch (trackerEvent.GetEventTypeString())
        {
            case "JUMP_START":
                jumpIndex_++;
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

    public void ResetEventQueue(int jumpIndex, int eventIndex)
    {
        indexEvent = eventIndex;
        jumpIndex_ = jumpIndex;
        JumpEndEvent event_ = (JumpEndEvent)(eventsQueue[eventIndex]);
        fishMovement.transform.position = event_.getPlayerPos();

        offset += Time.time - offset - jumpEndTimes[jumpIndex];
        fishMovement.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void NextJump()
    {
        if(jumpIndex_< jumpEndIndex.Count-1)
        {
            jumpIndex_++;
            UpdateFishJump();
        }

    }

    public void PrevJump()
    {
        if(jumpIndex_> 0)
        {
            jumpIndex_--;
            UpdateFishJump();
        }

    }

    private void UpdateFishJump()
    {
        indexEvent = jumpEndIndex[jumpIndex_];
        JumpEndEvent event_ = (JumpEndEvent)(eventsQueue[indexEvent]);
        fishMovement.transform.position = event_.getPlayerPos();
        offset += Time.time - offset - jumpEndTimes[jumpIndex_];
        fishMovement.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}

