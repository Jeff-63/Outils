using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class BallData
{
    [XmlAttribute("color")]
    public float[] color;

    [XmlAttribute("position")]
    public float[] position;

    [XmlAttribute("velocity")]
    public float[] velocity;

    public BallData() { }
    public BallData(float[] _color, float[] _position, float[] _velocity)
    {
        color = _color;
        position = _position;
        velocity = _velocity;
    }
}

[XmlRoot("Balls Collection")]
[System.Serializable]
public class BallsToSave
{
    [XmlArray("Balls"), XmlArrayItem("Ball")]
    public List<BallData> ballsDatas;

    public BallsToSave()
    {
    }

    public BallsToSave(List<BallData> _ballsDatas)
    {
        ballsDatas = _ballsDatas;
    }

}


public class Helper
{
    public static float[] V3ToFloatArray(Vector3 v3)
    {
        float[] retour = new float[3];

        retour[0] = v3.x;
        retour[1] = v3.y;
        retour[2] = v3.z;

        return retour;
    }

    public static Vector3 FloatArrayToV3(float[] tab)
    {
        return new Vector3(tab[0], tab[1], tab[2]);
    }

    public static float[] ColorToFloatArray(Color c)
    {
        float[] retour = new float[4];

        retour[0] = c.r;
        retour[1] = c.g;
        retour[2] = c.b;
        retour[3] = c.a;
        return retour;
    }

    public static Color FloatArrayToColor(float[] tab)
    {
        return new Color(tab[0], tab[1], tab[2], tab[3]);
    }
}