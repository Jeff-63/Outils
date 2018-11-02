using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public static void IgnoreColiTimed(this Collider2D collider, Collider2D target, float f)
    {
        IgnoreColiHelper.Instance.AddTimedIgnore(collider, target, f, true);
    }

    public static void MoveForward(this Transform transform,float speed, float dt)
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

}
