using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardAssetInfo))]
public class CardAssetInfoEditor : Editor
{
    private SerializedObject cardAssetInfo;

    private SerializedProperty name;
    private SerializedProperty mana;
    private SerializedProperty sprite;
    private SerializedProperty textDesc;
    private SerializedProperty color;

    Rect p, r;

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        Initialize();

        DrawTheCard();

        cardAssetInfo.ApplyModifiedProperties();

    }

    void Initialize()
    {
        cardAssetInfo = new SerializedObject(target);
        name = cardAssetInfo.FindProperty("name");
        mana = cardAssetInfo.FindProperty("mana");
        sprite = cardAssetInfo.FindProperty("sprite");
        textDesc = cardAssetInfo.FindProperty("textDesc");
        color = cardAssetInfo.FindProperty("color");
    }

    public void GetPosition(Rect position)
    {
        p = position;
    }

    void DrawBackground()
    {
        Rect backgroundCard = r;
        backgroundCard.width = (Screen.width / 2f);
        backgroundCard.height = (Screen.height / 2f);
        Color actualBackgroundColor = GUI.backgroundColor;
        Color actualColor = GUI.color;
        GUI.backgroundColor = color.colorValue;
        GUI.color = color.colorValue;
        GUI.DrawTexture(backgroundCard, Texture2D.whiteTexture);
        GUI.backgroundColor = actualBackgroundColor;
        GUI.color = actualColor;
    }

    void DrawImageField()
    {

        Rect imageField = r;
        imageField.width = (Screen.width / 3f);
        imageField.height = (Screen.height / 4f);
        imageField.x += Screen.width / 12;
        imageField.y += Screen.height / 16;

        sprite.objectReferenceValue = EditorGUI.ObjectField(imageField, sprite.objectReferenceValue, typeof(Sprite), true);
    }

    void DrawManaCostField()
    {
        Rect manaCostField = r;
        manaCostField.width = (Screen.width / 25f);
        manaCostField.height = (Screen.height / 35f);
        manaCostField.x += (Screen.width / 2.25f);
        manaCostField.y += (Screen.height / 100f);
        mana.intValue = EditorGUI.IntField(manaCostField, mana.intValue);
        if (mana.intValue <= 0)
            mana.intValue = 1;
        else if (mana.intValue > 5)
            mana.intValue = 1;
    }

    void DrawNameField()
    {
        Rect nameField = r;
        nameField.width = (Screen.width / 8f);
        nameField.height = (Screen.height / 35f);
        nameField.x += (Screen.width / 50);
        nameField.y += (Screen.height / 100f);


        GUI.SetNextControlName("NameField");
        name.stringValue = EditorGUI.TextField(nameField, name.stringValue);

        
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl().Equals("NameField"))        //small issue : need to press enter 2 time 
        {
            string path = "Assets/" + CardAssetInfo.CardAssetInfoPath + "NewCard.asset";
            AssetDatabase.RenameAsset(path, name.stringValue + ".asset");
        }
    }

    void DrawDescriptionField()
    {
        Rect descField = r;
        descField.width = (Screen.width / 3f);
        descField.height = (Screen.height / 8f);
        descField.x += (Screen.width / 12f);
        descField.y += (Screen.height / 3f);
        textDesc.stringValue = EditorGUI.TextArea(descField, textDesc.stringValue);
    }

    void DrawColorField()
    {
        Rect colorField = r;
        colorField.width = (Screen.width / 13f);
        colorField.height = (Screen.height / 26f);
        colorField.x += (Screen.width / 2.35f);
        colorField.y += (Screen.height / 2.2f);
        color.colorValue = EditorGUI.ColorField(colorField, color.colorValue);
    }

    void DrawTheCard()
    {
        GUILayout.BeginArea(new Rect((Screen.width / 2.8f) - 50, (Screen.height / 4f), (Screen.width), (Screen.height)));
        r = EditorGUILayout.GetControlRect();
        DrawBackground();
        DrawImageField();
        DrawManaCostField();
        DrawNameField();
        DrawDescriptionField();
        DrawColorField();
        GUILayout.EndArea();
    }
}
