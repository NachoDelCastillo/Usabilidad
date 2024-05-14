using System;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

[Serializable]
public abstract class TrackerEvent
{
    public static DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public enum EventType
    {
        SESSION_START, SESSION_END,
        GAME_START, GAME_END,
        JUMP_START, JUMP_END,
        MOVE_START, MOVE_END
    };

    //Parametros comunes
    string gameVersion;
    string userId;
    protected EventType eventType;
    double timeStamp;
    double localTimeStamp;

    public EventType Type() {
        return eventType;
    }

    protected TrackerEvent(EventType eventType)
    {
        gameVersion = Application.version;
        userId = GenerateUserId();
        this.eventType = eventType;

        localTimeStamp = Time.time;
        //Segundos que han pasado desde el 1/1/1970)
        timeStamp = (DateTime.UtcNow - epochStart).TotalSeconds;
    }

    protected TrackerEvent(string gameVesion, string userId, EventType eventType,
        double timeStamp, double localTimeStamp)
    {
        this.gameVersion = gameVesion;
        this.userId = userId;
        this.eventType = eventType;
        this.timeStamp = timeStamp;
        this.localTimeStamp = localTimeStamp;
    }

    private string GenerateUserId()
    {
        string uniqueAttribute = SystemInfo.deviceUniqueIdentifier;

        // Calcula el hash SHA-256 del atributo único
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(uniqueAttribute));

            // Convierte el hash en una cadena hexadecimal
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public virtual string ToCSV()
    {
        return string.Format("\n{0},{1},{2},{3}", gameVersion, userId, eventType, timeStamp.ToString(CultureInfo.InvariantCulture));
    }

    //No virtual
    public string ToJSON()
    {
        string returnValue = 
            
            "{\n"

            + string.Format(
            "\t\"gameVersion\": \"{0}\",\n" +
            "\t\"userID\": \"{1}\",\n" +
            "\t\"eventType\": \"{2}\",\n" +
            "\t\"timeStamp\": {3},\n"+
            "\t\"localTimeStamp\": {4}",
            gameVersion, userId, eventType.ToString(), timeStamp.ToString(CultureInfo.InvariantCulture),localTimeStamp.ToString(CultureInfo.InvariantCulture))

            + CompleteParameters()
            + "}";
        return returnValue;
    }

    protected virtual string CompleteParameters()
    {  return "\n"; }

    public double getTimeStamp()
    {
        return timeStamp;
    }

    public double getLocalTimeStamp() => localTimeStamp;

    public EventType GetEventType() { return eventType; }
    public string GetEventTypeString() { return eventType.ToString(); }
}
