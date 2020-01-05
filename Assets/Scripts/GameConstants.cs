using UnityEngine;

    public enum eDirection
    {
        Up,
        Down,
        Left,
        Right,
    }

public static partial class GameConstants
{
    public const float cellSize = 1.28f;

    public static Vector2 DirectionVector(eDirection direction)
    {
        switch (direction)
        {
            case eDirection.Up:
                return Vector2.up;
            case eDirection.Down:
                return Vector2.down;
            case eDirection.Left:
                return Vector2.left;
            case eDirection.Right:
                return Vector2.right;
        }
        return Vector2.zero;
    }

    public static float DirectionAngle(eDirection direction)
    {
        return -Vector2.Angle(Vector2.up, DirectionVector(direction));
    }
}
