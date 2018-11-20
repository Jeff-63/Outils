using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public static class ExtensionMethods
{

    public static int Factorial(this int i) 
    {
        return MathHelper.Factorial(i);
    }

    public static Vector2 AngToV2(this float v) //extend float with this
    {
        return MathHelper.DegreeToVector2(v);
    }

    public static string ArrToString<T>(this T[] arr)
    {
        string toRet = "[";
        for (int i = 0; i < arr.Length; ++i)
        {
            toRet += arr[i].ToString();
            if (i != arr.Length - 1)
                toRet += ",";
        }
        return toRet + "]";
    }

    public static Wrapper<T> Wrap<T>(this T toWrap) where T : struct
    {
        return new Wrapper<T>() { value = toWrap };
    }

    public static T[] GetArrOfEnums<T>(this T t) where T : struct, System.IConvertible //extend only struct and Iconvertible
    {
        return System.Enum.GetValues(typeof(T)).Cast<T>().ToArray<T>();
    }

    #region MyExtensions
    public static void IgnoreColiTimed(this Collider2D collider, Collider2D target, float f)
    {
        IgnoreColiHelper.Instance.AddTimedIgnore(collider, target, f, true);
    }

    public static void MoveForward(this Transform transform, float speed, float dt)
    {
        transform.Translate(transform.forward * speed * dt, Space.Self);
    }

    public static float V2ToAngle(this Vector2 v)
    {
        return MathHelper.Vector2ToAngle(v);
    }

    public static bool Compiles<T>(this T[] arr, System.Func<T, bool> func)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (!func(arr[i]))
                return false;
        }

        return true;
    }
    #endregion

    public static void Set(this RectTransform rt, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector4 offset)
    {
        rt.pivot = pivot;

        rt.offsetMin = new Vector2(offset.w, offset.x);
        rt.offsetMax = new Vector2(offset.y, offset.z);

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
    }

    public static Vector4 Set(this Vector4 v4, Vector2 part1, Vector2 part2)
    {
        v4.w = part1.x;
        v4.x = part1.y;
        v4.y = part2.x;
        v4.z = part2.y;
        return v4;
    }

    public static Vector2[] V4toTwoV2(this Vector4 v4)
    {
        Vector2[] toRet = new Vector2[2];
        toRet[0] = new Vector2(v4.w, v4.x);
        toRet[1] = new Vector2(v4.y, v4.z);
        return toRet;
    }
}
