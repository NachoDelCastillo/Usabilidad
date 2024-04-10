using System;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

[Serializable]
public abstract class TrackerEvent
{
    static DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public enum EventType
    {
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

    public EventType Type() {
        return eventType;
    }

    protected TrackerEvent(EventType eventType)
    {
        gameVersion = Application.version;
        userId = GenerateUserId();
        this.eventType = eventType;

        //Segundos que han pasado desde el 1/1/1970)
        timeStamp = (DateTime.UtcNow - epochStart).TotalSeconds;
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


    public virtual string ToJSON()
    {
        return string.Format(
            "\t\"gameVersion\": \"{0}\",\n" +
            "\t\"userID\": \"{1}\",\n" +
            "\t\"eventType\": \"{2}\",\n" +
            "\t\"timeStamp\": {3}",
            gameVersion, userId, eventType.ToString(), timeStamp.ToString(CultureInfo.InvariantCulture));
    }
}
