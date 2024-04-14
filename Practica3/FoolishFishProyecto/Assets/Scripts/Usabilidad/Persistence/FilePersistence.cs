using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class FilePersistence : IPersistence {
	private Queue<string> eventQueue = new Queue<string>();

	public void Send(TrackerEvent trackerEvent, ISerializer serializerObject, bool persistImmediately) {
		string serializedEvent = serializerObject.Serialize(trackerEvent);

		string fileFormat = serializerObject.getFormat();
		string filePath = Application.persistentDataPath + "/events" + fileFormat;

		if (persistImmediately) {
			PersistEvent(serializedEvent, fileFormat, filePath);
			Debug.Log("Evento persistido: " + trackerEvent.Type());
		}
		else {
			eventQueue.Enqueue(serializedEvent);
			Debug.Log("Evento encolado: " + trackerEvent.Type()); 
		}
	}

	public void Flush(ISerializer serializerObject) {
		string fileFormat = serializerObject.getFormat();
		string filePath = Application.persistentDataPath + "/events" + fileFormat;

		int enqueuedEvents = eventQueue.Count;
		while (eventQueue.Count > 0) {
			PersistEvent(eventQueue.Dequeue(), fileFormat, filePath);
		}
		Debug.Log("Persistidos " + enqueuedEvents + " eventos de la cola.");
	}

	void PersistEvent(string serializedEvent, string fileFormat, string filePath) {
		bool doesFileExist = File.Exists(filePath);

		//Crear el archivo de eventos o a�adir al que ya existe
		using (StreamWriter streamWriter = doesFileExist ? File.AppendText(filePath) : File.CreateText(filePath)) {
			switch (fileFormat) {
				case ".json": {
					//A�adimos la coma a excepcion de la primera vez
					string comma = doesFileExist ? ",\n" : string.Empty;

					streamWriter.Write(comma + serializedEvent);
				}
				break;
				case ".csv": {
					//A�adimos las columnas solo la primera vez
					string columns = doesFileExist ? string.Empty : "gameVersion,userID,eventType,timeStamp,arg1";

					streamWriter.Write(columns + serializedEvent);
				}
				break;
				default:
					throw new System.NotImplementedException("No se ha implementado el formato " + fileFormat);

			}
		}
	}
}
