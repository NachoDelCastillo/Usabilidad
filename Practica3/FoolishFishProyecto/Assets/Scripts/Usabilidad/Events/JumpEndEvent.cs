using UnityEngine;

public class JumpEndEvent : TrackerEvent
{ 
    // Id de la plataforma en la que se ha aterrizado
    int platformId;
    // Posicion exacta en la que el personaje ha aterrizado
    Vector2 playerPos;

    public JumpEndEvent(int platformId, Vector2 playerPos) : base(EventType.JUMP_END)
    {
        this.platformId = platformId;
        this.playerPos = playerPos;
    }

    public JumpEndEvent(string gameVersion, string userId, int platformId, Vector2 playerPos, double timeStamp) : base(gameVersion, userId, EventType.JUMP_END, timeStamp)
    {
        this.platformId = platformId;
        this.playerPos = playerPos;
    }

    public override string ToCSV()
    {
        return base.ToCSV() + string.Format(",{0}", platformId);
    }

    protected override string CompleteParameters()
    {
        string playerPosX = playerPos.x.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        string playerPosY = playerPos.y.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

        return ",\n" + string.Format(

            "\t \"platformId\": {0} ," + "\n" +

            // Almacenar la posicion exacta desde la que el personaje ha saltado
            "\t \"playerPosX\": {1} , " + "\n" +
            "\t \"playerPosY\": {2} \n"

            , platformId,
            playerPosX, playerPosY);
    }

    public int getPlatformId()
    {
        return platformId;
    }

    public Vector2 getPlayerPos()
    {
        return playerPos;
    }
}