using System;
using UnityEngine;

[Serializable]
public abstract class TrackerEvent
{
    static DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public enum EventType {
        SESSION_START, SESSION_END,
        GAME_START, GAME_END,
        JUMP_START, JUMP_END,
        PLAYER_MOVE
    };

    //Parametros comunes
    string gameVersion;
    string userId;
    protected EventType eventType;
    double timeStamp;

	protected TrackerEvent(EventType eventType) {
		gameVersion = Application.version;
        //userId = ;
        this.eventType = eventType;

		//Segundos que han pasado desde el 1/1/1970)
		timeStamp = (DateTime.UtcNow - epochStart).TotalSeconds; 
	}

	public virtual string ToCSV()
    {
        return "";
    }

    public virtual string ToJSON()
    {
		return string.Format(
			"\tgameVersion: \"{0}\"\n" +
			"\tuserID: \"{1}\"\n" +
			"\teventType: {2}\n" +
			"\ttimeStamp: {3}\n", 
            gameVersion, userId, eventType, timeStamp);
	} 
}
