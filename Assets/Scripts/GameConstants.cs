using UnityEngine;

public static partial class GameConstants
{
    public const float cellSize = 1.28f;
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

    public const int PlayerTanksCount = 4;
}
