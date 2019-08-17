
public class GameEventBase
{
    public enum EventType
    {
        Unknown,
        GuiEvent
    }

    public virtual EventType Type()
    {
        return EventType.Unknown;
    }
}

public class GuiEvent : GameEventBase
{
    override public EventType Type()
    {
        return EventType.GuiEvent;
    }
}

