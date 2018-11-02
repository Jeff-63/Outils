using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(SampleClass))]
public class SampleClassDrawer : PropertyDrawer
{

    private ReorderableList list;

    public bool initialized = false;
    Rect variableARect;
    readonly int LABEL_WIDTH = 50;
    public float variableAvalue;

    public void OnEnable(SerializedProperty property)
    {
        if (list == null)
        {
            list = BuildReorderableList(property);
        }
        initialized = true;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            OnEnable(property);
        }

        variableARect = RectBuilder.GetRect(0, 0, 15, 10, position.position, RectBuilder.ScalableType.ConstantWidth);
        list.DoList(variableARect);

        /*SerializedProperty a = property.FindPropertyRelative("a");

        //Rect Setup
        variableARect = RectBuilder.GetRect(0, 0, 4, 2, position.position);

        EditorGUI.LabelField(variableARect, "Var A : ");

        variableARect = RectBuilder.GetRect(4, 0, 10, 2, position.position);
        a.floatValue = EditorGUI.FloatField(variableARect, a.floatValue);
        a.floatValue = Mathf.Clamp(a.floatValue, 0, 100);*/

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            OnEnable(property);
        }

        return 100;
    }

    private ReorderableList BuildReorderableList(SerializedProperty property)
    {
        SerializedObject so = property.serializedObject;
        SerializedProperty sp = property.FindPropertyRelative("Stats");

        ReorderableList list = new ReorderableList(so, sp, true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Stats");
        };

        list.drawElementCallback = DrawPropertyField;
        return list;
    }

    private void DrawPropertyField(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        string s = element.propertyType.ToString();
        SerializedProperty s2 = element.FindPropertyRelative("a");
        rect.y += 2;
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("a"), GUIContent.none);
    }
}
