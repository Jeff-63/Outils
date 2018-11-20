using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

[CustomEditor(typeof(CardAssetInfo))]
public class CardEditorWindow : EditorWindow
{
    public enum SearchType { Name, Mana, Color }
    

    Object toPreview;
    string cardNameResearch = "";
    int manaCostMin, manaCostMax;
    Color colorSelected;
    List<CardAssetInfo> allCard;
    List<CardAssetInfo> cardList;
    Vector2 scrollPosition;
    Sprite image;
    CardAssetInfo actualCard;
    int indiceActualCard;
    bool layoutDefaultStatus;

    [MenuItem("Custom Windows/Card Editor")]
    public static void OpenWindow()
    {
        GetWindow<CardEditorWindow>();
    }

    private void OnEnable()
    {
        allCard = GetAllCard();
        cardList = new List<CardAssetInfo>();
        minSize = new Vector2(500, 650);
    }

    public void OnGUI()
    {
        layoutDefaultStatus = GUI.enabled;
        DrawSearchZone();

        DrawUILine();

        EditorGUILayout.Space();

        DrawCardZone();

    }

    public void DrawSearchZone()
    {
        float defaultLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        cardNameResearch = EditorGUILayout.TextField("Name : ", cardNameResearch);
        if (GUILayout.Button("Search"))
        {
            if (cardNameResearch != null)
            {
                cardList = Search(cardNameResearch.ToUpper(), manaCostMin, manaCostMax, colorSelected, SearchType.Name);
                actualCard = null;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        manaCostMin = int.Parse(EditorGUILayout.TextField("Mana Cost : ", manaCostMin.ToString()));
        EditorGUIUtility.labelWidth = 12f;
        manaCostMax = int.Parse(EditorGUILayout.TextField("-", manaCostMax.ToString()));
        if (GUILayout.Button("Search"))
        {
            cardList = Search(cardNameResearch, manaCostMin, manaCostMax, colorSelected, SearchType.Mana);
            actualCard = null;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = defaultLabelWidth;

        EditorGUILayout.BeginHorizontal();
        colorSelected = EditorGUILayout.ColorField(colorSelected);
        if (GUILayout.Button("Search"))
        {
            cardList = Search(cardNameResearch, manaCostMin, manaCostMax, colorSelected, SearchType.Color);
            actualCard = null;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(70));
        for (int i = 0; i < cardList.Count; i++)
        {
            if (GUILayout.Button(cardList[i].name))
            {
                indiceActualCard = i;
                actualCard = cardList[indiceActualCard];
                Selection.activeObject = actualCard;
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        DrawUILine();

        GUIContent btnTxt = new GUIContent("Create");
        var rt = GUILayoutUtility.GetRect(btnTxt, GUI.skin.button, GUILayout.ExpandWidth(false));
        rt.center = new Vector2(EditorGUIUtility.currentViewWidth / 2, rt.center.y);

        if (GUI.Button(rt, btnTxt, GUI.skin.button))
        {
            actualCard = CreateAsset();
        }

    }

    public void DrawCardZone()
    {
        if (actualCard != null)
        {
            EditorGUILayout.BeginHorizontal();//GUILayout.Width
            if (cardList.Count < 2)
                GUI.enabled = false;
            if (GUILayout.Button("<<", GUILayout.Width(30), GUILayout.Height((Screen.height / 1.3f))))
            {
                if (cardList.Count > 0)
                {
                    if (indiceActualCard > 0)
                    {
                        indiceActualCard--;
                    }
                    else
                    {
                        indiceActualCard = cardList.Count - 1;
                    }
                    actualCard = cardList[indiceActualCard];
                    Selection.activeObject = actualCard;
                }
            }
            GUI.enabled = layoutDefaultStatus;

            if (actualCard != null)
                ShowTheCard(actualCard, EditorGUILayout.GetControlRect());

            if (cardList.Count < 2)
                GUI.enabled = false;
            if (GUILayout.Button(">>", GUILayout.Width(30), GUILayout.Height(Screen.height / 1.3f)))
            {
                if (cardList.Count > 0)
                {
                    if (indiceActualCard < cardList.Count - 1)
                    {
                        indiceActualCard++;
                    }
                    else
                    {
                        indiceActualCard = 0;
                    }
                    actualCard = cardList[indiceActualCard];
                    Selection.activeObject = actualCard;
                }
            }
            GUI.enabled = layoutDefaultStatus;

            EditorGUILayout.EndHorizontal();
        }
    }

    public static void DrawUILine(int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, Color.black);
    }

    public List<CardAssetInfo> Search(string name, int manaCostMin, int manaCostMax, Color color, SearchType searchType)
    {
        switch (searchType)
        {
            case SearchType.Name:
                return SearchByName(name);
            case SearchType.Mana:
                return SearchByMana(this.manaCostMin, manaCostMax);
            case SearchType.Color:
                return SearchByColor(color);
            default:
                Debug.Log("Unhandeled Value of " + searchType);
                break;
        }

        return null;
    }

    public List<CardAssetInfo> SearchByName(string name)
    {
        List<CardAssetInfo> result = new List<CardAssetInfo>();
        foreach (CardAssetInfo card in allCard)
        {
            if (card.name.Contains(name))
            {
                result.Add(card);
            }
        }
        return result;
    }

    public List<CardAssetInfo> SearchByMana(int manaCostMin, int manaCostMax)
    {
        List<CardAssetInfo> result = new List<CardAssetInfo>();
        foreach (CardAssetInfo card in allCard)
        {
            int manaCost = card.mana;
            if (manaCost >= manaCostMin && manaCost <= manaCostMax)
            {
                result.Add(card);
            }
        }
        return result;
    }

    public List<CardAssetInfo> SearchByColor(Color color)
    {
        List<CardAssetInfo> result = new List<CardAssetInfo>();
        foreach (CardAssetInfo card in allCard)
        {
            if (card.color == color)
            {
                result.Add(card);
            }
        }
        return result;
    }

    public List<CardAssetInfo> GetAllCard()
    {
        CardAssetInfo[] all = Resources.LoadAll<CardAssetInfo>("CardAssetInfo");

        if (all != null)
            return all.ToList();
        else
            return null;
    }

    public void ShowTheCard(CardAssetInfo card, Rect p)
    {
        CardAssetInfoEditor editor = (CardAssetInfoEditor)Editor.CreateEditor(card);
        editor.GetPosition(p);
        editor.OnInspectorGUI();
    }

    private CardAssetInfo CreateAsset()
    {
        CardAssetInfo asset = ScriptableObject.CreateInstance<CardAssetInfo>();
        string s = Path.Combine(Application.dataPath, CardAssetInfo.CardAssetInfoPath);
        if (Directory.Exists(s))
        {
            AssetDatabase.CreateAsset(asset, "Assets/" + CardAssetInfo.CardAssetInfoPath + "NewCard.asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
            GetAllCard();
        }

        return asset;
    }
}
