using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper {

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static int Factorial(int i)
    {
        if (i <= 1)
            return 1;
        return Factorial(i - 1) * i;
    }


    public static float Vector2ToAngle(Vector2 vector2)
    {
        if (vector2.x < 0)
        {
            return  -Vector2.Angle(new Vector2(1, 0), vector2);
        }
        else
        {
            return Vector2.Angle(new Vector2(1, 0), vector2);
        }

    }
}
