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
		//Crear el archivo de eventos o añadir al que ya existe
		bool doesFileExist = File.Exists(filePath);

		FileInfo fileInfo = new FileInfo(filePath);
		using (StreamWriter streamWriter = new(fileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))) {
			switch (fileFormat) {
				case ".json": {
					if (doesFileExist) {
						//Sustituimos el ultimo corchete por una coma
						streamWriter.BaseStream.Seek(-1, SeekOrigin.End);
						streamWriter.Write(",\n");
					}
					else {
						//Escribimos el primer corchete
						streamWriter.Write("[");
					}

					streamWriter.Write(serializedEvent + "]");
				}
				break;
				case ".csv": {
					//Añadimos las columnas solo la primera vez
					string columns = doesFileExist ? string.Empty : "gameVersion,userID,eventType,timeStamp,arg1";

					//Escribimos siempre al final (append)
					streamWriter.BaseStream.Seek(0, SeekOrigin.End);
					streamWriter.Write(columns + serializedEvent);
				}
				break;
				default:
					throw new System.NotImplementedException("No se ha implementado el formato " + fileFormat);

			}
		}
	}
}
