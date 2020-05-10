using UnityEngine;

public static partial class GameConstants
{
    public const float CELL_SIZE = 1.28f;
    public const int MAX_PLAYERS = 4;

    public const float BUILD_THRESHOLD_MAX = 1.5f;
    public const float BUILD_THRESHOLD_MIN = 0.5f;

    public const int CONCRETE_WALL_TIMER = 12;
    public const float CONCRETE_WALL_BLINK_TIMER = 3.5f;
    public const float CONCRETE_WALL_CHANGE_TEXTURE_TIMER = 0.5f;

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
}
