using System;
using UnityEngine;
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
        switch (direction)
        {
            case Direction.Up:
                return 90.0f;
            case Direction.Down:
                return -90.0f;
            case Direction.Left:
                return 180.0f;
            case Direction.Right:
                return 0.0f;
        }
        return 0.0f;
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

    public static Direction InvertDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
        }
        return direction;
    }

    public static Direction ClockwiseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Right;
            case Direction.Down:
                return Direction.Left;
            case Direction.Left:
                return Direction.Up;
            case Direction.Right:
                return Direction.Down;
        }
        return direction;
    }

    public static Direction CounterClockwiseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Left;
            case Direction.Down:
                return Direction.Right;
            case Direction.Left:
                return Direction.Up;
            case Direction.Right:
                return Direction.Down;
        }
        return direction;
    }

    public static Direction DirectionToTarget(Transform source, Transform target)
    {
        float xDir = target.position.x - source.position.x;
        float yDir = target.position.y - source.position.y;

        if (Math.Abs(xDir) > Math.Abs(yDir))
        {
            if (xDir < 0.0)
                return Direction.Left;
            else
                return Direction.Right;
        }
        else
        {
            if (yDir < 0.0)
                return Direction.Down;
            else
                return Direction.Up;
        }
    }

    public static float Rand()
    {
        var r = new System.Random();
        float value = (float)r.NextDouble();
        return value;
    }

    public static int Rand(int min, int max)
    {
        var r = new System.Random();
        return r.Next(min, max);
    }
}
