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

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999))
        {
            Debug.LogWarning("insert value between 1 and 3999");
            return number.ToString();
        }
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return number.ToString();
    }
}
