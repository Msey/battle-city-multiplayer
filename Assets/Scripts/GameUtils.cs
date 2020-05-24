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

    public static GameObject[] ToGameObjects(this MonoBehaviour[] objects)
    {
        var gameObjects = new GameObject[objects.Length];

        for (int i = 0; i < objects.Length; i++)
            gameObjects[i] = objects[i].gameObject;

        return gameObjects;
    }

    public static Vector2[] ToVectors(this GameObject[] objects)
    {
        var vectors = new Vector2[objects.Length];

        for (int i = 0; i < objects.Length; i++)
            vectors[i] = objects[i].transform.position;

        return vectors;
    }


    public static Vector3 RandomPointInBounds3D(Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public static Vector2 RandomPointInBounds2D(Bounds bounds)
    {
        return new Vector2(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y)
        );
    }



    public static Vector2 RandomPointInVectors2D(
        Vector2 left, Vector2 right, 
        Vector2 top, Vector2 bottom, 
        float offsetFactorX = 0,
        float offsetFactorY = 0)
    {
        return new Vector2(
            UnityEngine.Random.Range(left.x + offsetFactorX, right.x - offsetFactorX),
            UnityEngine.Random.Range(top.y - offsetFactorY, bottom.y + offsetFactorY)
        );
    }

    public static Vector2 RandomPointInRect(Rect rect, float roundFactor, float offsetFactorX = 0, float offsetFactorY = 0)
    {
        return new Vector2(
             Utils.RoundByFactor(UnityEngine.Random.Range(rect.xMin + offsetFactorX, rect.xMax - offsetFactorX), roundFactor),
             Utils.RoundByFactor(UnityEngine.Random.Range(rect.yMin + offsetFactorY, rect.yMax - offsetFactorY), roundFactor)
        );
    }

    public static Vector2 GetLeftVector2D(this Vector2[] bunch)
    {
        float maxLeft = float.PositiveInfinity;
        int leftVectorIndex = -1;

        for (int i = 0; i < bunch.Length; i++)
        {
            if (bunch[i].x < maxLeft)
            {
                maxLeft = bunch[i].x;
                leftVectorIndex = i;
            }
        }

        return bunch[leftVectorIndex];
    }

    public static Vector2 GetRightVector2D(this Vector2[] bunch)
    {
        float maxRight = float.NegativeInfinity;
        int rightVectorIndex = -1;

        for (int i = 0; i < bunch.Length; i++)
        {
            if (bunch[i].x > maxRight)
            {
                maxRight = bunch[i].x;
                rightVectorIndex = i;
            }
        }

        return bunch[rightVectorIndex];
    }


    public static Vector2 GetTopVector2D(this Vector2[] bunch)
    {
        float maxTop = float.NegativeInfinity;
        int topVectorIndex = -1;

        for (int i = 0; i < bunch.Length; i++)
        {
            if (bunch[i].y > maxTop)
            {
                maxTop = bunch[i].y;
                topVectorIndex = i;
            }
        }

        return bunch[topVectorIndex];
    }

    public static Vector2 GetBottomVector2D(this Vector2[] bunch)
    {
        float maxBottom = float.PositiveInfinity;
        int bottomVectorIndex = -1;

        for (int i = 0; i < bunch.Length; i++)
        {
            if (bunch[i].y < maxBottom)
            {
                maxBottom = bunch[i].y;
                bottomVectorIndex = i;
            }
        }

        return bunch[bottomVectorIndex];
    }

    public static Rect ComputeOverlapRect(this Vector2[] bunch)
    {
        float xMax = float.NegativeInfinity;
        float yMax = float.NegativeInfinity;
        float xMin = float.PositiveInfinity;
        float yMin = float.PositiveInfinity;

        foreach (Vector2 point in bunch)
        {
            if (point.x < xMin)
                xMin = point.x;
            else if (point.x > xMax)
                xMax = point.x;

            if (point.y < yMin)
                yMin = point.y;
            else if (point.y > yMax)
                yMax = point.y;
        }

        Rect overlapRect = new Rect();
        overlapRect.xMax = xMax;
        overlapRect.yMax = yMax;
        overlapRect.xMin = xMin;
        overlapRect.yMin = yMin;
        return overlapRect;
    }
}
