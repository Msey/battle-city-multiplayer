using UnityEngine;

public static partial class GameConstants
{
    public const float cellSize = 1.28f;
    public enum eDirection
    {
        Up,
        Down,
        Left,
        Right,
    }

    public static Vector2 DirectionVector(eDirection direction)
    {
        switch (direction)
        {
            case GameConstants.eDirection.Up:
                return Vector2.up;
            case GameConstants.eDirection.Down:
                return Vector2.down;
            case GameConstants.eDirection.Left:
                return Vector2.left;
            case GameConstants.eDirection.Right:
                return Vector2.right;
        }
        return Vector2.zero;
    }

    public static float DirectionAngle(eDirection direction)
    {
        return -Vector2.Angle(Vector2.up, GameConstants.DirectionVector(direction));
    }
}
