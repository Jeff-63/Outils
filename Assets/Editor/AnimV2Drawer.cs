using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AnimV2))]
public class AnimV2Drawer : PropertyDrawer
{
    bool initializedDrawer;

    const float displayPropertyWidth = 125f;

    public void OnEnable(SerializedProperty property)
    {
        initializedDrawer = true;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initializedDrawer)
            OnEnable(property);


        SerializedProperty animCurve = property.FindPropertyRelative("animCurve");
        SerializedProperty animBounds = property.FindPropertyRelative("animBounds");

        EditorGUI.BeginProperty(position, label, property);
        int indentSaved = EditorGUI.indentLevel;
        float widthFromIndentation = indentSaved * 9f;
        EditorGUI.indentLevel = 0;

        Rect labelRect = new Rect(widthFromIndentation + position.x, position.y, 180, 20);
        Rect leftRect = new Rect(widthFromIndentation + position.x + 180, position.y, 60, 20);
        Rect rightRect = new Rect(widthFromIndentation + position.x + 240, position.y, 120, 20);

        EditorGUI.LabelField(labelRect, property.name);
        animCurve.animationCurveValue = EditorGUI.CurveField(leftRect, "", animCurve.animationCurveValue);
        animBounds.vector2Value = EditorGUI.Vector2Field(rightRect, "", animBounds.vector2Value);

        EditorGUI.EndProperty();
        EditorGUI.indentLevel = indentSaved;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!initializedDrawer)
            OnEnable(property);

        return 20; 
    }
}
