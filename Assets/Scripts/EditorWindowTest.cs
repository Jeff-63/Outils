using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorWindowTest : EditorWindow {

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("DANGER!", MessageType.Warning);            //Creates a help box
        EditorGUILayout.TextArea("heyo");
    }
}
