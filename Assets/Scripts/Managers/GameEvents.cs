
public abstract class GameEventBase
{
    public enum EventType
    {
        Unknown,
        GuiEvent,
        KeyBoardEvent,
        KeyPressEvent
    }

    public abstract EventType Type();
}

public class GuiEvent : GameEventBase
{
    override public EventType Type()
    {
        return EventType.GuiEvent;
    }
}

public class KeyBoardEvent : GameEventBase
{
    override public EventType Type()
    {
        return EventType.KeyBoardEvent;
    }
}

public class KeyPressEvent : GameEventBase
{
    override public EventType Type()
    {
        return EventType.KeyPressEvent;
    }

    public readonly UnityEngine.KeyCode code;

    public KeyPressEvent(UnityEngine.KeyCode code)
    {
        this.code = code;
    }
}

