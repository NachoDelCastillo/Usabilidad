using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public class FilePersistence : IPersistence {
	ISerializer serializerObject;
	Queue<TrackerEvent> eventQueue;
	string filePath;
	StreamWriter streamWriter;

    private Thread persistenceThread;
	private bool firstEvent;
	private bool doesFileExist;

    public FilePersistence(ISerializer serializerObject) {
		this.serializerObject = serializerObject;
		eventQueue = new Queue<TrackerEvent>();

		string fileFormat = serializerObject.getFormat();
		filePath = Application.persistentDataPath + "/events" + fileFormat;

		//Crear el archivo de eventos o añadir al que ya existe
		doesFileExist = File.Exists(filePath);
		FileInfo fileInfo = new FileInfo(filePath);
		streamWriter = new(fileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite));

		firstEvent = true;

		if(!doesFileExist)
			streamWriter.Write(serializerObject.Header());
		else
			streamWriter.BaseStream.Seek(serializerObject.SeekEndOffset(), SeekOrigin.End);
	}

	public void Send(TrackerEvent trackerEvent, bool persistImmediately) {

		if (persistImmediately) {
            PersistInmediatly(trackerEvent);
			Debug.Log("Evento persistido: " + trackerEvent.Type());
		}
		else {
			eventQueue.Enqueue(trackerEvent);
			Debug.Log("Evento encolado: " + trackerEvent.Type()); 
		}
	}

	public void Flush() {
        if (persistenceThread != null && persistenceThread.IsAlive)
        {
            persistenceThread.Join();
        }

        persistenceThread = new Thread(new ThreadStart(ThreadPersistEvents));

		persistenceThread.Start();
    }

	void PersistEvent(TrackerEvent trackerEvent) {
		//Escribir en el fichero
		streamWriter.Write(serializerObject.Prefix(firstEvent && !doesFileExist));
		streamWriter.Write(serializerObject.Serialize(trackerEvent));
		streamWriter.Write(serializerObject.Suffix());

		firstEvent = false;
	}

	public void SetSerializerObject(ISerializer serializerObject) {
		this.serializerObject = serializerObject;
	}

	private void ThreadPersistEvents()
	{
        int enqueuedEvents = eventQueue.Count;
        while (eventQueue.Count > 0)
        {
            PersistEvent(eventQueue.Dequeue());
        }
        Debug.Log("Persistidos " + enqueuedEvents + " eventos de la cola.");
    }

	private void PersistInmediatly(TrackerEvent trackerEvent)
	{
        if (persistenceThread != null && persistenceThread.IsAlive)
        {
            persistenceThread.Join();
        }

        persistenceThread = new Thread(new ParameterizedThreadStart(ThreadPersistEvent));

        persistenceThread.Start(trackerEvent);
    }

	private void ThreadPersistEvent(object trackerEvent)
	{
        TrackerEvent _event = (TrackerEvent) trackerEvent;
		PersistEvent(_event);
    }

    public void Close()
    {
        if (persistenceThread != null && persistenceThread.IsAlive)
        {
            persistenceThread.Join();
        }

        streamWriter.Write(serializerObject.EndOfFile());

        streamWriter.Close();
    }
}
