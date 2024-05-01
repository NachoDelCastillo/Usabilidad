using UnityEngine;

public class JumpStartEvent : TrackerEvent
{
    // Id de la plataforma desde la que se salta
    int platformId;
    // Posicion exacta desde la que el personaje ha saltado
    Vector2 playerPos;
    // Posicion del raton con la que se calcula la parabola
    Vector2 mousePos;

    public JumpStartEvent(int platformId, Vector2 playerPos, Vector2 mousePos): base(EventType.JUMP_START)
    {
        this.platformId = platformId;
        this.playerPos = playerPos;
        this.mousePos = mousePos;

        Debug.Log("mousePosX = " + mousePos.x + " // mousePosY = " + mousePos.y);
    }

    public JumpStartEvent(string gameVersion, string userId, int platformId, Vector2 mousePos, Vector2 playerPos, double timeStamp) : base(gameVersion, userId, EventType.JUMP_START, timeStamp)
    {
        this.platformId = platformId;
        this.mousePos = mousePos;
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

        string mousePosX = mousePos.x.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        string mousePosY = mousePos.y.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

        return ",\n" + string.Format(

            "\t \"platformId\": {0} ," + "\n" +

            // Almacenar la posicion exacta desde la que el personaje ha saltado
            "\t \"playerPosX\": {1} , " + "\n" +
            "\t \"playerPosY\": {2} , " + "\n" +

            // Almacenar la posicion del raton con la que se calcula la parabola
            "\t \"mousePosX\": {3} , " + "\n" +
            "\t \"mousePosY\": {4} \n"

            , platformId,
            playerPosX, playerPosY,
            mousePosX, mousePosY);
    }

    public int getPlatformId()
    {
        return platformId;
    }

    public Vector2 getPlayerPos()
    {
        return playerPos;
    }

    public Vector2 getMousePos()
    {
        return mousePos;
    }
}
