using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class XMLSerializer{

    #region Singleton
    private static XMLSerializer instance = null;

    private XMLSerializer()
    {
    }

    public static XMLSerializer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new XMLSerializer();
            }
            return instance;
        }
    }
    #endregion


    public void SaveXMLDataToDisk(string filePath, object toSave)
    {
        string path = Application.streamingAssetsPath + "/" + filePath + ".xml";
        var serializer = new XmlSerializer(typeof(BallsToSave));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, toSave);
        }
    }

    public T LoadBinaryDataFromDisk<T>(string filePath)
    {
        string path = Application.streamingAssetsPath + "/" + filePath + ".xml";
        var serializer = new XmlSerializer(typeof(T));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return (T)serializer.Deserialize(stream);
        }
    }

}
