using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

[CustomPropertyDrawer(typeof(ObjectPlacer))]
public class ObjectPlacerDrawerOld : PropertyDrawer
{

    public bool initialized = false;
    //List<string> enumAsString;
    List<string> prefabsList;
    int selectedPopUpIndex;
    Camera cam;
    Vector3 camPosition;


    public void OnEnable(SerializedProperty property)
    {
        /*List<TestEnum> lt = System.Enum.GetValues(typeof(TestEnum)).Cast<TestEnum>().ToList<TestEnum>();
        enumAsString = System.Enum.GetNames(typeof(TestEnum)).ToList();
        selectedPopUpIndex = property.FindPropertyRelative("testEnum").enumValueIndex;*/
        string datePath = Application.dataPath + "/Resources/Prefabs";
        prefabsList = Directory.GetFiles(datePath, "*.prefab").ToList();

        initialized = true;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Event evt = Event.current;
        if (!initialized)
        {
            OnEnable(property);
        }

        Rect drawerRect = RectBuilder.GetRect(0, 0, 17, 10, position.position, RectBuilder.ScalableType.ConstantWidth);
        GUI.Box(drawerRect, "Object Tool");

        //List
        drawerRect = RectBuilder.GetRect(4, 2, 10, 2, position.position, RectBuilder.ScalableType.ConstantWidth);
        initializePrefabsList(prefabsList.ToArray(), drawerRect);

        //Button
        drawerRect = RectBuilder.GetRect(4, 4, 10, 2, position.position, RectBuilder.ScalableType.ConstantWidth);
        bool activeObject = Selection.activeObject;
        if (GUI.Button(drawerRect, activeObject ? "Swap" : "Create"))
        {
            string activePath = "Prefabs/" + prefabsList[selectedPopUpIndex];
            GameObject newObject = new GameObject();
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

            SceneView sv = SceneView.lastActiveSceneView;
            if (sv)
                cam = SceneView.lastActiveSceneView.camera;
            if (cam)
                camPosition = cam.ViewportToWorldPoint(newObject.transform.position);
            else
                camPosition = Vector3.zero;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            OnEnable(property);
        }

        return 200;
    }

    void initializePrefabsList(string[] paths, Rect drawerRect)
    {
        int length = paths.Length;
        string[] fileNames = new string[length];

        for (int i = 0; i < length; i++)
        {
            fileNames[i] = Path.GetFileName(paths[i]).Replace(".prefab", "");
            prefabsList[i] = fileNames[i];
        }

        selectedPopUpIndex = EditorGUI.Popup(drawerRect, selectedPopUpIndex, fileNames);
    }

    public GameObject GetPrefab(string path)
    {
        GameObject go = Resources.Load<GameObject>(path);
        return go;
    }

}
