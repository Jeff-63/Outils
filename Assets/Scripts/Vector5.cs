using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector5
{
    public delegate bool dlg(float i);

    public float v, w, x, y, z;
    public List<GameObject> gameObjects;
    private float o;

    public Vector5(float _v, float _w, float _x, float _y, float _z)
    {
        v = _v;
        w = _w;
        x = _x;
        y = _y;
        z = _z;
    }

    public Vector5 ShallowClone()
    {
        return this.MemberwiseClone() as Vector5;
    }

    public void setO(float _o)
    {
        o = _o;
    }

    public bool Exist(dlg deleg)
    {
        return deleg.Invoke(v) || deleg.Invoke(w) || deleg.Invoke(x) || deleg.Invoke(y) || deleg.Invoke(z);
    }

    public bool OneExist(System.Func<float,bool> func)
    {
        return func.Invoke(v) || func.Invoke(w) || func.Invoke(x) || func.Invoke(y) || func.Invoke(z);
    }

    #region Operator overload

    public static Vector5 operator +(Vector5 a, Vector5 b)
    {
        return new Vector5(
            a.v + b.v,
            a.w + b.w,
            a.x + b.x,
            a.y + b.y,
            a.z + b.z);
    }

    public static Vector5 operator *(Vector5 a, float b)
    {
        return new Vector5(
            a.v * b,
            a.w * b,
            a.x * b,
            a.y * b,
            a.z * b);
    }

    public static bool operator ==(Vector5 a, Vector5 b)
    {
        return (
        Mathf.Approximately(a.v, b.v) &&
        Mathf.Approximately(a.w, b.w) &&
        Mathf.Approximately(a.x, b.x) &&
        Mathf.Approximately(a.y, b.y) &&
        Mathf.Approximately(a.z, b.z));
    }

    public static bool operator !=(Vector5 a, Vector5 b)
    {
        return !(a == b);
    }

    //surcharge[]
    public float this[int i]
    {
        get
        {

            switch (i)
            {
                case 0:
                    return v;
                case 1:
                    return w;
                case 2:
                    return x;
                case 3:
                    return y;
                case 4:
                    return z;
                default:
                    throw new System.IndexOutOfRangeException("Vector 5 Exception : max index 5 exceed !!");
            }
        }
        set
        {
            switch (i)
            {
                case 0:
                    v = value;
                    break;
                case 1:
                    w = value;
                    break;
                case 2:
                    x = value;
                    break;
                case 3:
                    y = value;
                    break;
                case 4:
                    z = value;
                    break;
                default:
                    throw new System.IndexOutOfRangeException("Vector 5 Exception : max index 5 exceed !!");
            }
        }
    }

    public override string ToString()
    {
        return string.Format("Vector 5({0},{1},{2},{3},{4})", v, w, x, y, z);
    }
    #endregion
}
