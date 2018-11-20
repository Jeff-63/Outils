using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class Serializer : MonoBehaviour {

    public enum DataType { Binary, XML, Json }

    #region Singleton
    private static Serializer instance = null;

    private Serializer()
    {
    }

    public static Serializer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Serializer();
            }
            return instance;
        }
    }
    #endregion

    public void SaveDataToDisk(string filePath, object toSave, DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Binary:
                BinarySerializer.Instance.SaveBinaryDataToDisk(filePath, toSave);
                break;
            case DataType.XML:
                XMLSerializer.Instance.SaveXMLDataToDisk(filePath, toSave);
                break;
            case DataType.Json:
               JsonSerializer.Instance.SaveJsonDataToDisk(filePath, toSave);
                break;
            default:
                break;
        }
    }

    public T LoadDataFromDisk<T>(string filePath, DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Binary:
                return BinarySerializer.Instance.LoadBinaryDataFromDisk<T>(filePath);
            case DataType.XML:
                return XMLSerializer.Instance.LoadBinaryDataFromDisk<T>(filePath); ;
            case DataType.Json:
                return JsonSerializer.Instance.LoadJsonDataFromDisk<T>(filePath);
            default:
                return default(T);
        }
    }

    public void ClearFile(string path)
    {
        if (File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
    }

    public string[] GetDataTypes()
    {
        return Enum.GetNames(typeof(DataType)).ToArray();
    }
}
