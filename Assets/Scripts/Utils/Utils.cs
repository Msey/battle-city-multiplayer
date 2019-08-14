using System;
using UnityEngine;
using UnityEngine.Assertions;

public static partial class Utils
{
    public static bool Verify<T>(T value) where T : class
    {
        Assert.IsNotNull(value, "Null reference error");
        return (value != null);
    }

    public static bool Verify(bool value)
    {
        Assert.IsTrue(value, "True assert error");
        return value;
    }


    public static void Unwrap(this Tuple<Vector2, bool> r, out Vector2 direction, out bool execution )
    {
        direction = r.Item1;
        execution = r.Item2;
    }

    public static T UseComponent<T>(this GameObject extractFrom) where T : Component
    {
        return extractFrom.GetComponent<T>() ?? extractFrom.AddComponent<T>();
    }

    public static bool InRange(int left, int value, int right) 
    {
        return (left <= value) && (value < right);
    }

    public static bool InRange(float left, float value, float right)
    {
        return (left <= value) && (value < right);
    }

    public static Vector2 AngleAsVector2(this float angle)
    {
        return new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
    }
}
