using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericClassCreator {

    public static string[] GetAllFileNamesInFolder(string path, string ext, bool withPaths = true, System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
    {
        string[] allFiles = System.IO.Directory.GetFiles(path,"*." + ext);
        if (!withPaths)
            for (int i = 0; i < allFiles.Length; i++)
                allFiles[i] = System.IO.Path.GetFileNameWithoutExtension(allFiles[i]);
        return allFiles;
    }

    public static Dictionary<string,Type> GetAllTypesFromFileNames(string[] fileNames)
    {
        Dictionary<string, Type> toRet = new Dictionary<string, Type>();
        for (int i = 0; i < fileNames.Length; i++)
        {
            string corrected = System.IO.Path.GetFileNameWithoutExtension(fileNames[i]);
            toRet.Add(corrected, Type.GetType(corrected));
        }
        return toRet;
    }

    public static Dictionary<string, Type> GetAllTypesFromFileLocation(string loc)
    {
        return GetAllTypesFromFileNames(GetAllFileNamesInFolder(loc, "cs"));
    }
}
