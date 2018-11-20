using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BasicPreviewWindow : EditorWindow {

    Object toPreview;

    [MenuItem("Custom Windows/Preview")]
	public static void OpenWindow()
    {
        GetWindow<BasicPreviewWindow>();
    }

    public void OnGUI()
    {
        //toPreview = Ed 
        if (GUILayout.Button("Load Asset"))
        {
            string absPath = EditorUtility.OpenFilePanel("Select Asset", "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relativePath = absPath.Replace(Application.dataPath, "");
                relativePath = "Assets" + relativePath;
                toPreview = AssetDatabase.LoadAssetAtPath(relativePath, typeof(Object)) as Object;
                if(toPreview != null)
                {
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = toPreview;
                }
            }
        }

        toPreview = EditorGUILayout.ObjectField("ToPreview", toPreview, typeof(Object), false) as Object;
        if (toPreview != null)
        {
            Editor editor = Editor.CreateEditor(toPreview);
            editor.OnInspectorGUI();
        }
    }
}




/*
 * string relPath = AssetDatabase.GetAssetPath(inventoryItemList);

{
string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
inventoryItemList = AssetDatabase.LoadAssetAtPath(relPath, typeof(CardAssetInfoItemList)) as CardAssetInfoItemList;
}

string[] aFilePaths = Directory.GetFiles(onlyFolderPath);
[MenuItem("Custom Tools/ Color Window")]
EditorUtility.FocusProjectWindow();
Selection.activeObject = inventoryItemList;
string absPath = EditorUtility.OpenFilePanel ("Select Inventory Item List", "", "");
        if (absPath.StartsWith(Application.dataPath)) 

inventoryItemList = AssetDatabase.LoadAssetAtPath (relPath, typeof(InventoryItemList)) as InventoryItemList;
*/
