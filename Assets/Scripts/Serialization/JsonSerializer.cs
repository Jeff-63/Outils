using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSerializer{

    #region Singleton
    private static JsonSerializer instance = null;

    private JsonSerializer()
    {
    }

    public static JsonSerializer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new JsonSerializer();
            }
            return instance;
        }
    }
    #endregion

    public void SaveJsonDataToDisk(string filePath, object toSave)
    {
        JsonManager.SaveObjectAsJSONToStreamingAsset(toSave, filePath);
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
}
