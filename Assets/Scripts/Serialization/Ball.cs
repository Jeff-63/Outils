using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    Vector3 position, velocity;
    Color color;
    readonly int POS_MIN = -10, POS_MAX = 10;

    public void SetupRand()
    {
        position = new Vector3(Random.Range(POS_MIN, POS_MAX), Random.Range(POS_MIN, POS_MAX), Random.Range(POS_MIN, POS_MAX));
        gameObject.transform.position = position;
        color = Random.ColorHSV();
        gameObject.GetComponent<Renderer>().material.color = color;
        velocity = new Vector3(Random.Range(POS_MIN, POS_MAX), Random.Range(POS_MIN, POS_MAX), Random.Range(POS_MIN, POS_MAX));
        gameObject.GetComponent<Rigidbody>().velocity = velocity;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    public BallData ExportAsBallData()
    {
        color = gameObject.GetComponent<Renderer>().material.color;
        velocity = gameObject.GetComponent<Rigidbody>().velocity;
        position = gameObject.transform.position;

        return new BallData(Helper.ColorToFloatArray(color), Helper.V3ToFloatArray(position), Helper.V3ToFloatArray(velocity));

    }

    public void ImportBallData(BallData ballData)
    {
        position = Helper.FloatArrayToV3(ballData.position);
        gameObject.transform.position = position;
        color = Helper.FloatArrayToColor(ballData.color);
        gameObject.GetComponent<Renderer>().material.color = color;
        velocity = Helper.FloatArrayToV3(ballData.velocity);
        gameObject.GetComponent<Rigidbody>().velocity = velocity;
        
    }
}
