using UnityEngine;

    public enum Direction
    {
        Undefined,
        Up,
        Down,
        Left,
        Right,
    }

public static partial class GameConstants
{
    public const float cellSize = 1.28f;

    public static Vector2 DirectionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector2.up;
            case Direction.Down:
                return Vector2.down;
            case Direction.Left:
                return Vector2.left;
            case Direction.Right:
                return Vector2.right;
        }
        return Vector2.zero;
    }

    public static float DirectionAngle(Direction direction)
    {
        return -Vector2.Angle(Vector2.up, DirectionVector(direction));
    }
}
