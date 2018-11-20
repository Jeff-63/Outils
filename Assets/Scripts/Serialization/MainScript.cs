using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("Main")]
public class MainScript : MonoBehaviour
{

    int nbBalls;
    readonly int NB_MIN = 1, NB_MAX = 15;
    Color color;
    Vector3 velocity, position;
    GameObject go, parent;
    List<Ball> balls;
    BallsToSave ballsDatas;
    BallData bdate;

    string fileName;
    int selectedTypeDate;

    // Use this for initialization
    void Start()
    {
        SetupInitGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        string[] dataTypes = Serializer.Instance.GetDataTypes();
        GUI.Label(new Rect(0, 0, 100, 20), "Data Type : ");
        selectedTypeDate = GUI.SelectionGrid(new Rect(100, 0, 500, 20), selectedTypeDate, dataTypes, dataTypes.Length);
        GUI.Label(new Rect(0, 20, 100, 20), "File Name : ");
        fileName = GUI.TextField(new Rect(100, 20, 500, 20), fileName);

        if (GUI.Button(new Rect(0, 40, 100, 50), "Save"))
        {
            Save();
        }

        if (GUI.Button(new Rect(100, 40, 100, 50), "Load"))
        {
            Load();
        }
    }

    public void SetupInitGame()
    {
        balls = new List<Ball>();
        ballsDatas = new  BallsToSave(new List<BallData>());

        nbBalls = UnityEngine.Random.Range(NB_MIN, NB_MAX);
        parent = new GameObject("Balls");
        for (int i = 0; i < nbBalls; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.AddComponent<Rigidbody>();
            go.transform.parent = parent.transform;
            go.AddComponent<Ball>();
            go.GetComponent<Rigidbody>().useGravity = false;
            Ball ball = go.GetComponent<Ball>();
            ball.SetupRand();
            balls.Add(ball);
        }
    }

    public void Save()
    {
        
        if (fileName != null && fileName != "")
        {
            ballsDatas.ballsDatas.Clear();
            CreateBallsDatas();
            //CreateABall();
            Serializer.Instance.SaveDataToDisk(fileName, ballsDatas, (Serializer.DataType)selectedTypeDate);
            Debug.Log("Save successful");
        }
        else
        {
            Debug.Log("FileName Missing");
        }
    }

    public void Load()
    {

        if (fileName != null && fileName != "")
        {
            balls.Clear();
            ballsDatas.ballsDatas.Clear();
            ballsDatas = Serializer.Instance.LoadDataFromDisk<BallsToSave>(fileName, (Serializer.DataType)selectedTypeDate);

            if (parent)
            {
                //destroy all child
                foreach (Transform child in parent.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            //load all new children
            foreach (BallData ballData in ballsDatas.ballsDatas)
            {
                CreateBall(ballData);
            }
        }
        else
        {
            Debug.Log("FileName Missing");
        }
    }

    public void CreateBall(BallData ballData)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.AddComponent<Rigidbody>();
        go.transform.parent = parent.transform;
        go.AddComponent<Ball>();
        go.GetComponent<Rigidbody>().useGravity = false;

        Ball ball = go.GetComponent<Ball>();
        balls.Add(ball);

        SetGOPosition(go, ballData);
        SetGOColor(go, ballData);
        SetGOVelocity(go, ballData);

    }

    void SetGOPosition(GameObject go, BallData ballData)
    {
        go.transform.position = Helper.FloatArrayToV3(ballData.position);
    }

    void SetGOColor(GameObject go, BallData ballData)
    {
        color = Helper.FloatArrayToColor(ballData.color);
        go.GetComponent<Renderer>().material.color = color;
    }

    void SetGOVelocity(GameObject go, BallData ballData)
    {
        velocity = Helper.FloatArrayToV3(ballData.velocity);
        go.GetComponent<Rigidbody>().velocity = velocity;
    }

    public void CreateBallsDatas()
    {
        foreach (Ball b in balls)
        {
            bdate = b.ExportAsBallData();
            ballsDatas.ballsDatas.Add(bdate);
        }
    }

    public void CreateABall()
    {
        bdate = new BallData();
        Vector3 test = new Vector3(5, 5, 5);
        bdate.color = Helper.ColorToFloatArray(new Color(5, 5, 5, 5));
        bdate.velocity = Helper.V3ToFloatArray(test);
        bdate.position = Helper.V3ToFloatArray(test);

        ballsDatas.ballsDatas.Add(bdate);
    }
}
