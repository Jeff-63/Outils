using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public CardAssetInfo cardAssetInfo;

    public Text nameText;
    public Text mana;
    public Image spriteImg;
    public Text desc;
    public Color color;

    public void Start()
    {
        nameText.text = cardAssetInfo.name;
        mana.text = cardAssetInfo.mana.ToString();
        spriteImg.sprite = cardAssetInfo.sprite;
        desc.text = cardAssetInfo.textDesc;
        color = cardAssetInfo.color;
    }
}
