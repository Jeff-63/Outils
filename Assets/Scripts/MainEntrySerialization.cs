using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class MainEntrySerialization : MonoBehaviour
{ 
    public enum DataType { Binary, XML, Json }

    List<Ball> listBall = new List<Ball>();
    int nbBalls;
    float posX, posY, posZ;
    readonly int NB_MIN = 1, NB_MAX = 15, POS_MIN = -10, POS_MAX = 10;
    Color color;
    Vector3 velocity, position;
    GameObject go, parent;
    string fileName;
    int selectedTypeDate;

    // Use this for initialization
    void Start()
    {
        nbBalls = UnityEngine.Random.Range(NB_MIN, NB_MAX);
        parent = new GameObject("Balls");
        for (int i = 0; i < nbBalls; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            position = new Vector3(UnityEngine.Random.Range(POS_MIN, POS_MAX), UnityEngine.Random.Range(POS_MIN, POS_MAX), UnityEngine.Random.Range(POS_MIN, POS_MAX));
            go.transform.position = position;
            color = UnityEngine.Random.ColorHSV();
            go.GetComponent<Renderer>().material.color = color;
            go.transform.parent = parent.transform;
            velocity = new Vector3(UnityEngine.Random.Range(POS_MIN, POS_MAX), UnityEngine.Random.Range(POS_MIN, POS_MAX), UnityEngine.Random.Range(POS_MIN, POS_MAX));
            go.AddComponent<Rigidbody>();
            go.GetComponent<Rigidbody>().velocity = velocity;
            go.GetComponent<Rigidbody>().useGravity = false;
        }
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

        string[] dataTypes = GetDataTypes();
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

    public string[] GetDataTypes()
    {
        return Enum.GetNames(typeof(DataType)).ToArray();
    }

    public void SaveDataToDisk(string filePath, object toSave, DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Binary:
                SaveBinaryDataToDisk(filePath, toSave);
                break;
            case DataType.XML:
                break;
            case DataType.Json:
                SaveJsonDataToDisk(filePath, toSave);
                break;
            default:
                break;
        }
    }

    public void SaveBinaryDataToDisk(string filePath, object toSave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.streamingAssetsPath + "/" + filePath;
        ClearFile(path);
        FileStream file = File.Create(path);
        bf.Serialize(file, toSave);
        file.Close();
    }

    public void SaveJsonDataToDisk(string filePath, object toSave)
    {
        JsonManager.SaveObjectAsJSONToStreamingAsset(toSave, filePath);
    }

    public T LoadDataFromDisk<T>(string filePath, DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Binary:
                return LoadBinaryDataFromDisk<T>(filePath);
            case DataType.XML:
                return default(T);
            case DataType.Json:
                return LoadJsonDataFromDisk<T>(filePath);
            default:
                return default(T);
        }
    }

    public T LoadBinaryDataFromDisk<T>(string filePath)
    {
        T toRet;
        string path = Application.streamingAssetsPath + "/" + filePath;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            toRet = (T)bf.Deserialize(file);
            file.Close();
        }
        else
            toRet = default(T);
        return toRet;
    }

    public T LoadJsonDataFromDisk<T>(string filePath)
    {
        T toRet;
        string path = Application.streamingAssetsPath + "/" + filePath;
        if (File.Exists(path))
        {
            toRet = JsonManager.LoadJsonFromStreamingAssetFile<T>(filePath + ".json");

        }
        else
            toRet = default(T);
        return toRet;
    }

    public void ClearFile(string path)
    {
        if (File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
    }

    public void Save()
    {
        if (fileName != null && fileName != "")
        {
            listBall.Clear();
            foreach (Transform t in parent.transform)
            {
                color = t.gameObject.GetComponent<Renderer>().material.color;
                position = t.gameObject.transform.position;
                velocity = t.gameObject.GetComponent<Rigidbody>().velocity;
                listBall.Add(new Ball(new MyColor(color), new V3(position), new V3(velocity)));
            }

            SaveDataToDisk(fileName, listBall, (DataType)selectedTypeDate);

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
            listBall.Clear();
            listBall = LoadDataFromDisk<List<Ball>>(fileName, (DataType)selectedTypeDate);

            if (parent)
            {
                //destroy all child
                foreach (Transform child in parent.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            //load all new children
            foreach (Ball ball in listBall)
            {
                CreateBall(ball);
            }
        }
        else
        {
            Debug.Log("FileName Missing");
        }
    }

    public void CreateBall(Ball ball)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.parent = parent.transform;

        SetGOPosition(go, ball);
        SetGOColor(go, ball);
        SetGOVelocity(go, ball);

    }

    void SetGOPosition(GameObject go, Ball ball)
    {
        
        go.transform.position = ball.position.ToVector3();
    }

    void SetGOColor(GameObject go, Ball ball)
    {
        color = ball.color.ToColor();
        go.GetComponent<Renderer>().material.color = color;
    }

    void SetGOVelocity(GameObject go, Ball ball)
    {
        velocity = ball.velocity.ToVector3();
        go.AddComponent<Rigidbody>();
        go.GetComponent<Rigidbody>().velocity = velocity;
        go.GetComponent<Rigidbody>().useGravity = false;
    }

    [System.Serializable]
    public class Ball
    {

        public MyColor color;
        public V3 position, velocity;

        public Ball(MyColor _color, V3 _position, V3 _velocity)
        {
            color = _color;
            position = _position;
            velocity = _velocity;
        }

        public override string ToString()
        {
            return "OK";
        }
    }

    [System.Serializable]
    public class MyColor
    {
        public float r, g, b, a;

        public MyColor(Color c)
        {
            r = c.r;
            g = c.g;
            b = c.b;
            a = c.a;
        }

        public Color ToColor()
        {
            return new Color(r, g, b, a);
        }
    }

    [System.Serializable]
    public class V3
    {
        public float x, y, z;

        public V3(Vector3 v3)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }

}
