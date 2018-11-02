using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

[CustomPropertyDrawer(typeof(ObjectPlacer))]
public class ObjectPlacerDrawer : PropertyDrawer
{

    public bool initialized = false;

    //enum names popup list
    string[] enumsAsString;
    int selectedPopupIndex;

    List<FileInfo> goList;
    int goIndex;
    List<string> prefabNamesList;

    public void OnEnable(SerializedProperty property)
    {
        #region Notes
        //Cast<>() returns an IEnumerable of  
        //List<EditorProperty> enums = System.Enum.GetValues(typeof(EditorProperty)).Cast<EditorProperty>().ToList<EditorProperty>(); 

        //OR

        //get list of names... google "get enums as list c#" for more info
        //enumsAsString = System.Enum.GetNames(typeof(EditorProperty)).ToArray();
        #endregion

        initialized = true;

        goList = new List<FileInfo>();
        prefabNamesList = new List<string>();
        DirectoryInfo d = new DirectoryInfo(Application.dataPath + "/Resources/Prefabs");
        FileInfo[] files = d.GetFiles("*.prefab"); //Getting prefabs

        foreach (FileInfo file in files)
        {
            goList.Add(file);
            prefabNamesList.Add(file.Name.Replace(".prefab",""));
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
            OnEnable(property);

        Rect testRect = RectBuilder.GetRect(0, 0, 10, 1, position.position, RectBuilder.ScalableType.ConstantWidth);
        Rect testRect2 = RectBuilder.GetRect(0, 2, 10, 1, position.position, RectBuilder.ScalableType.ConstantWidth);
        Rect testRect3 = RectBuilder.GetRect(0, 4, 10, 1, position.position, RectBuilder.ScalableType.ConstantWidth);

        EditorGUI.LabelField(testRect, "Object Tool");
        selectedPopupIndex = EditorGUI.Popup(testRect2, selectedPopupIndex, prefabNamesList.ToArray());

        if (PrefabUtility.GetPrefabObject(Selection.activeGameObject))
        {
            if (GUI.Button(testRect3, "Swap"))
            {
                Debug.Log(goList[selectedPopupIndex].FullName);

                //destroy prefab then create new one
                GameObject.DestroyImmediate(Selection.activeGameObject);
                Selection.activeGameObject = GameObject.Instantiate(Resources.Load<GameObject>(goList[selectedPopupIndex].FullName) as GameObject);
            }

        }
        else
        {
            if (GUI.Button(testRect3, "Create"))
            {


            }

        }
        /*
       // if (Event.current.type == EventType.Repaint) {
            //EditorGUI.DrawTextureTransparent(testRect, );

        //}
        */
    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!initialized)
            OnEnable(property);

        return 100;
    }
}
