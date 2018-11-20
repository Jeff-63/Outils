using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Create asset menu makes it so you can right click in editor and select "Create" to create this script type
[CreateAssetMenu(fileName = "Character Desc", menuName = "Characters/Basic")] 
public class CardAssetInfo : ScriptableObject {

    public static string CardAssetInfoPath = "Resources/CardAssetInfo/";

    public new string name; //Since unity already uses name, the keyword new allows you to overwrite it
    public int mana;
    public Sprite sprite;
    public string textDesc;
    public Color color;


    public override string ToString()
    {
        return string.Format("CardAssetInfo [Name:{0},Sprite:{1},desc:{2}", name, sprite, textDesc);
    }
}

//It is like a monobehaviour, do not use New keyword or constructor
//OnEnable is best for initialization, it does not use all monobehaviour calls
//ScriptableObject.CreateInstance() to create a instance of one, otherwise it is by reference (use T or system type, string is reflection and not scalable!)
