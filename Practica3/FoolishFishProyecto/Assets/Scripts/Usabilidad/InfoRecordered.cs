using LLlibs.ZeroDepJson;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoRecordered : MonoBehaviour {
	// Devuelve true si se esta visualizando una repeticion de una partida
	static public bool playingRecordedGame { private set; get; }

	Queue<TrackerEvent> eventsQueue;
	private const int INVALID = -1;

	double timeStart, timeEnd, offset;

	[Serializable]
	public class EventBase {
		public string gameVersion;
		public string userID;
		public string eventType;
		public double timeStamp;
		public double localTimeStamp;
		public int platformId = INVALID;
		public int moveDirection = INVALID;
		public bool gameCompleted = false;
		public float mousePosX;
		public float mousePosY;
		public float playerPosX;
		public float playerPosY;
	}

	// Referencias
	// Referencia al script de movimiento del personaje principal
	FishMovement fishMovement;
	[SerializeField]
	private Slider progressBar;
	[SerializeField] GameObject markerPrefab;

	void Start() {
		if (!Tracker.Instance.ReplayMode) {
			playingRecordedGame = false;
			gameObject.SetActive(false);
			progressBar.gameObject.SetActive(false);
			return;
		}

		// Debug
		playingRecordedGame = true;
		fishMovement = FindAnyObjectByType<FishMovement>();

		eventsQueue = new Queue<TrackerEvent>();
		readFile();

		progressBar.gameObject.SetActive(true);
		progressBar.value = 0;

		offset = Time.time;
	}

	void readFile() {
		// Ruta del archivo CSV en Application.persistentDataPath
		string filePath = Path.Combine(Application.persistentDataPath, "events.json");

		// Verifica que el archivo exista antes de intentar leerlo
		if (File.Exists(filePath)) {
			// Lee el contenido del archivo JSON como una cadena
			string jsonContent = File.ReadAllText(filePath);

			EventBase[] events = ZVJson.FromJson<EventBase>(jsonContent, true);

			foreach (EventBase event_ in events) {
				string gameVersion = event_.gameVersion;
				string userID = event_.userID;
				double timeStamp = event_.timeStamp;
				double localTimeStamp = event_.localTimeStamp;

				TrackerEvent trackerEvent = null;

				switch (event_.eventType) {
					case "GAME_START":
						timeStart = localTimeStamp;
						break;
					case "GAME_END":
						timeEnd = localTimeStamp;
						Debug.Log("TimeEnd : " + timeEnd);
						break;
					case "JUMP_START":
						trackerEvent = new JumpStartEvent(gameVersion, userID, event_.platformId,
							new Vector2(event_.mousePosX, event_.mousePosY),
							new Vector2(event_.playerPosX, event_.playerPosY), timeStamp, localTimeStamp);
						break;
					case "JUMP_END":
						trackerEvent = new JumpEndEvent(gameVersion, userID, event_.platformId,
							new Vector2(event_.playerPosX, event_.playerPosY), timeStamp, localTimeStamp);
						break;
					case "MOVE_START":
						trackerEvent = new MoveStartEvent(gameVersion, userID,
							(MoveStartEvent.MoveDirection)event_.moveDirection, timeStamp, localTimeStamp);
						break;
					case "MOVE_END":
						trackerEvent = new MoveEndEvent(gameVersion, userID, timeStamp, localTimeStamp);
						break;
					default:
						continue;
				}

				if (trackerEvent != null)
					eventsQueue.Enqueue(trackerEvent);

				if (event_.eventType == "GAME_END")
					break;
			}

			foreach (TrackerEvent trackerEvent in eventsQueue) {
				if (trackerEvent.Type() != TrackerEvent.EventType.JUMP_END)
					continue;

				Vector2 markerPosition = new Vector2((float)
					((trackerEvent.getLocalTimeStamp() - timeStart) / timeEnd * 0.96f + 0.011f) * progressBar.GetComponent<RectTransform>().sizeDelta.x, 0);

				Instantiate(markerPrefab, progressBar.transform)
					.GetComponent<RectTransform>().anchoredPosition = markerPosition;
			}

			//for(int i = 0; i <= 10; i++) {
			//	Vector2 markerPosition = new Vector2((float)
			//		(((float) i / 10 * 0.96f + 0.011f) * progressBar.GetComponent<RectTransform>().sizeDelta.x), 0);

			//	Instantiate(markerPrefab, progressBar.transform)
			//		.GetComponent<RectTransform>().anchoredPosition = markerPosition;
			//}
		}
		else {
			Debug.LogWarning($"El archivo CSV no se encontró en la ruta: {filePath}");
		}
	}

	void Update() {
		progressBar.value = (float)((Time.time - offset) / timeEnd);

		if (eventsQueue.Count > 0) {
			// Obtener el primer evento de la cola sin quitarlo
			TrackerEvent nextEvent = eventsQueue.Peek();

			// Obtener el tiempo actual del juego   
			double currentGameTime = Time.time - offset;

			// Si el tiempo del próximo evento es menor o igual al tiempo actual del juego
			if (nextEvent.getLocalTimeStamp() - timeStart <= currentGameTime) {
				// Procesar el evento y quitarlo de la cola
				ProcessEvent(eventsQueue.Dequeue());
			}
		}
	}

	void ProcessEvent(TrackerEvent trackerEvent) {
		switch (trackerEvent.GetEventTypeString()) {
			case "GAME_END":
				Debug.Log("RecordedEvent : GameEnd");
				GameManager.GetInstance().ChangeScene("ReplayMenu");
				break;

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
