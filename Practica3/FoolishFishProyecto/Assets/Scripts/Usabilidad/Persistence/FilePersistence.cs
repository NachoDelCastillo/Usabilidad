using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class FilePersistence : IPersistence {
	ISerializer serializerObject;
	Queue<TrackerEvent> eventQueue;
	string filePath;
	StreamWriter streamWriter;

	public FilePersistence(ISerializer serializerObject) {
		this.serializerObject = serializerObject;
		eventQueue = new Queue<TrackerEvent>();

		string fileFormat = serializerObject.getFormat();
		filePath = Application.persistentDataPath + "/events" + fileFormat;

		//Crear el archivo de eventos o añadir al que ya existe
		bool doesFileExist = File.Exists(filePath);
		FileInfo fileInfo = new FileInfo(filePath);
		streamWriter = new(fileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite));

		if(!doesFileExist)
			streamWriter.Write(serializerObject.Header());
		else
			streamWriter.BaseStream.Seek(serializerObject.SeekEndOffset(), SeekOrigin.Begin);
	}

	~FilePersistence() {
		streamWriter.Write(serializerObject.EndOfFile());

		streamWriter.Close();
	}

	public void Send(TrackerEvent trackerEvent, bool persistImmediately) {

		if (persistImmediately) {
			PersistEvent(trackerEvent);
			Debug.Log("Evento persistido: " + trackerEvent.Type());
		}
		else {
			eventQueue.Enqueue(trackerEvent);
			Debug.Log("Evento encolado: " + trackerEvent.Type()); 
		}
	}

	public void Flush() {
		int enqueuedEvents = eventQueue.Count;
		while (eventQueue.Count > 0) {
			PersistEvent(eventQueue.Dequeue());
		}
		Debug.Log("Persistidos " + enqueuedEvents + " eventos de la cola.");
	}

	void PersistEvent(TrackerEvent trackerEvent) {
		//Escribir en el fichero
		streamWriter.Write(serializerObject.Prefix());
		streamWriter.Write(serializerObject.Serialize(trackerEvent));
		streamWriter.Write(serializerObject.Suffix());
	}

	public void SetSerializerObject(ISerializer serializerObject) {
		this.serializerObject = serializerObject;
	}
}
