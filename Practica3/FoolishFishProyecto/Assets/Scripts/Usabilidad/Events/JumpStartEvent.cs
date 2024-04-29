using UnityEngine;

public class JumpStartEvent : TrackerEvent
{
    int platformId;
    Vector2 mousePos;

    public JumpStartEvent(int platformId, Vector2 mousePos): base(EventType.JUMP_START)
    {
        this.platformId = platformId;
        this.mousePos = mousePos;

        Debug.Log("mousePosX = " + mousePos.x + " // mousePosY = " + mousePos.y);
    }

    public JumpStartEvent(string gameVersion, string userId, int platformId, Vector2 mousePos, double timeStamp) : base(gameVersion, userId, EventType.JUMP_START, timeStamp)
    {
        this.platformId = platformId;
        this.mousePos = mousePos;
    }

    public override string ToCSV()
    {
        return base.ToCSV() + string.Format(",{0}", platformId);
    }

    protected override string CompleteParameters()
    {
        string mousePosX = mousePos.x.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        string mousePosY = mousePos.y.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

        return ",\n" + string.Format(

            "\t \"platformId\": {0} ," + "\n" +
            "\t \"mousePosX\": {1} , " + "\n" +
            "\t \"mousePosY\": {2} \n"

            , platformId, mousePosX, mousePosY);
    }

    public int getPlatformId()
    {
        return platformId;
    }

    public Vector2 getMousePos()
    {
        return mousePos;
    }
}
