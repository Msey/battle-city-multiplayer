﻿using UnityEngine;
using static GameConstants;

public static partial class GameUtils
{
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

    public static bool IsDirectionAxisChanged(Direction oldDir, Direction newDir)
    {
        return IsVerticalAxis(oldDir) != IsVerticalAxis(newDir);
    }

    public static bool IsVerticalAxis(Direction dir)
    {
        return dir == Direction.Up || dir == Direction.Down;
    }

    public static bool IsHorizontalAxis(Direction dir)
    {
        return !IsVerticalAxis(dir);
    }
}
