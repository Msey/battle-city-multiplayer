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

    public enum PickUpType
    {
        Tank,
        Helmet,
        Star,
        Shovel,
        Clock,
        Grenade,
        Pistol
    }

    public enum EnemyTankType
    {
        Basic,
        Fast,
        Power,
        Armor,
    }

    public enum MapElementType
    {
        Nothing,
        Concrete,
        Forest,
        Brick,
        Water,
        Ice
    }

    public enum BuildSide
    {
        Up = Direction.Up,
        Down = Direction.Down,
        Left = Direction.Left,
        Right = Direction.Right,
        Full,
        Point
    }
}
