using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BinarySerializer{

    #region Singleton
    private static BinarySerializer instance = null;

    private BinarySerializer()
    {
    }

    public static BinarySerializer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BinarySerializer();
            }
            return instance;
        }
    }
    #endregion

    public void SaveBinaryDataToDisk(string filePath, object toSave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.streamingAssetsPath + "/" + filePath;
        Serializer.Instance.ClearFile(path);
        FileStream file = File.Create(path);
        bf.Serialize(file, toSave);
        file.Close();
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
}
