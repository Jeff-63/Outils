using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class MakeScriptableObject
{
    public static readonly string CardAssetInfoPath = "Resources/CardAssetInfos/NewBasicCardInfo.asset";


    [MenuItem("CardAssetInfo/Create New/Basic")]
    public static void CreateMyAsset()
    {
        CardAssetInfo asset = ScriptableObject.CreateInstance<CardAssetInfo>();                                 //Create an instance of an asset

        string s = Path.Combine(Application.dataPath, CardAssetInfoPath);

        //if (!Directory.Exists(Path.Combine(Application.dataPath,CardAssetInfoPath)))
        //    Directory.CreateDirectory(Path.Combine(Application.dataPath, CardAssetInfoPath));
        AssetDatabase.CreateAsset(asset, "Assets" + "/" + CardAssetInfoPath);             //Creates the object in the project
        AssetDatabase.SaveAssets();                                                                             //Write all unsaved objects to disc

        EditorUtility.FocusProjectWindow();                                                                     //Focus on the "Project" window

        Selection.activeObject = asset;                                                                         //Make the newly created object the selected one
    }
}
/*This is the old way of creating assets, now we can use  [CreateAssetMenu(fileName = "Character Desc", menuName = "Characters/Basic")]  that we saw inside CardAssetInfo
However, There are many useful functions to showcase here.
*/