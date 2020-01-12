using UnityEngine;

public static partial class GameUtils
{
    public static Vector2 DirectionVector(GameConstants.Direction direction)
    {
        switch (direction)
        {
            case GameConstants.Direction.Up:
                return Vector2.up;
            case GameConstants.Direction.Down:
                return Vector2.down;
            case GameConstants.Direction.Left:
                return Vector2.left;
            case GameConstants.Direction.Right:
                return Vector2.right;
        }
        return Vector2.zero;
    }

    public static float DirectionAngle(GameConstants.Direction direction)
    {
        return -Vector2.Angle(Vector2.up, GameUtils.DirectionVector(direction));
    }

    public static bool IsDirectionAxisChanged(GameConstants.Direction oldDir, GameConstants.Direction newDir)
    {
        return IsVerticalAxis(oldDir) != IsVerticalAxis(newDir);
    }

    public static bool IsVerticalAxis(GameConstants.Direction dir)
    {
        return dir == GameConstants.Direction.Up || dir == GameConstants.Direction.Down;
    }

    public static bool IsHorizontalAxis(GameConstants.Direction dir)
    {
        return !IsVerticalAxis(dir);
    }
}
