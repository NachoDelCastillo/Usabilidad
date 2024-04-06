using System;
using UnityEngine;

public class TrackerEvent
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
    double secondsSinceEpoch;

    protected TrackerEvent() {
        gameVersion = Application.version;
		//userId = ;

		//Segundos que han pasado desde el 1/1/1970)
		secondsSinceEpoch = (DateTime.UtcNow - epochStart).TotalSeconds; 
	}

	public string ToCSV()
    {
        return "";
    }

    public string ToJSON()
    {
        return "";
    } 
}
