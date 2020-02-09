
public class KeyBoardEvent
{

}

public class KeyPressEvent
{
    public readonly UnityEngine.KeyCode code;

    public KeyPressEvent(UnityEngine.KeyCode code)
    {
        this.code = code;
    }
}

public class LevelStartedEvent
{
}

public class LevelEndedEvent
{
}

