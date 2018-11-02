using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class ObjectPlacerEditor : EditorWindow
{
    [MenuItem("Custom Tools/ Object Placer Editor")]
    public static void CreateShowcase() //Menu items can call STATIC functions, does not work for non-static since Editor scripts cannot be attached to objects
    {
        EditorWindow window = GetWindow<ObjectPlacerEditor>("Object Placer Editor");
    }

    public bool initialized = false;

    //enum names popup list
    string[] enumsAsString;
    int selectedPopUpIndex;


    List<FileInfo> goList;
    int goIndex;
    List<string> prefabNamesList;
    Camera cam;
    Vector3 camPosition;

    public void OnEnable()
    {
        #region Notes
        //Cast<>() returns an IEnumerable of  
        //List<EditorProperty> enums = System.Enum.GetValues(typeof(EditorProperty)).Cast<EditorProperty>().ToList<EditorProperty>(); 

        //OR

        //get list of names... google "get enums as list c#" for more info
        //enumsAsString = System.Enum.GetNames(typeof(EditorProperty)).ToArray();
        #endregion

        goList = new List<FileInfo>();
        prefabNamesList = new List<string>();
        DirectoryInfo d = new DirectoryInfo(Application.dataPath + "/Resources/Prefabs");
        FileInfo[] files = d.GetFiles("*.prefab"); //Getting prefabs

        foreach (FileInfo file in files)
        {
            goList.Add(file);
            prefabNamesList.Add(file.Name.Replace(".prefab", ""));
        }
    }

    public void OnGUI()
    {
        SceneView sv = SceneView.lastActiveSceneView;
        if (sv)
        {
            cam = sv.camera;
        }
        if (cam)
            camPosition = cam.ViewportToWorldPoint(new Vector3(.5f, .5f, 8));
        else
            camPosition = Vector3.zero;

        Rect drawerRect = RectBuilder.GetRect(0, 0, 17, 10, position.position, RectBuilder.ScalableType.ConstantWidth);
        GUILayout.Label("Object Tool");

        //List
        drawerRect = RectBuilder.GetRect(4, 2, 10, 2, position.position, RectBuilder.ScalableType.ConstantWidth);
        initializePrefabsList(prefabNamesList.ToArray(), drawerRect);

        //Button
        drawerRect = RectBuilder.GetRect(4, 4, 10, 2, position.position, RectBuilder.ScalableType.ConstantWidth);
        bool activeObject = Selection.activeObject;
        if (GUILayout.Button(activeObject ? "Swap" : "Create"))
        {
            string activePath = "Prefabs/" + prefabNamesList[selectedPopUpIndex];
            GameObject newObject = null;
            if (activeObject) //Swap
            {
                var selection = Selection.gameObjects;

                for (var i = selection.Length - 1; i >= 0; --i)
                {
                    var selected = selection[i];
                    newObject = GameObject.Instantiate(GetPrefab(activePath)) as GameObject;

                    newObject.transform.parent = selected.transform.parent;
                    newObject.transform.localPosition = selected.transform.localPosition;
                    newObject.transform.localRotation = selected.transform.localRotation;
                    newObject.transform.localScale = selected.transform.localScale;
                    newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());

                    GameObject.DestroyImmediate(selected);
                }
            }
            else //Create
            {
                newObject = GameObject.Instantiate(GetPrefab(activePath), camPosition, Quaternion.identity) as GameObject;
            }
        }
    }

    void initializePrefabsList(string[] paths, Rect drawerRect)
    {
        selectedPopUpIndex = EditorGUILayout.Popup(selectedPopUpIndex, paths);
    }

    public GameObject GetPrefab(string path)
    {
        GameObject go = Resources.Load<GameObject>(path);
        return go;
    }

}
