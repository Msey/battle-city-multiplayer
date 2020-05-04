using UnityEngine;

public static partial class GameConstants
{
    public const float CELL_SIZE = 1.28f;
    public const int MAX_PLAYERS = 4;

    public enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }

    public enum GameState
    {
        NotStarted,
        Loading,
        Started,
        PreFinished,
        Finished,
    }

    public enum GroupType
    {
        Other,
        Enemies,
        Players
    }
}
