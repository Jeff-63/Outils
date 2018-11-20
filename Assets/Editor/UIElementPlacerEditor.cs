using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIElementPlacerEditor : EditorWindow
{

    public enum UIElementType { LifeBar, MiniMap, Portrait }
    public enum UIElementPosition { TopLeft, TopRight, BottomLeft, BottomRight }
    float lifeDefaultValue = 0;
    UIElementType selectedPopUpTypeIndex;
    UIElementPosition selectedPopUpPositionIndex;
    Canvas canvas;
    Sprite lifeBarImage, portrait;
    Camera miniMapCamera;

    Vector2 TopLeftPositionMin, TopRightPositionMin, BottomLeftPositionMin, BottomRightPositionMin,
        TopLeftPositionMax, TopRightPositionMax, BottomLeftPositionMax, BottomRightPositionMax;

    readonly Vector2 Pivot = Vector2.zero, Offset = Vector2.zero;

    [MenuItem("Custom Tools/UIElement Placer Editor")]
    public static void CreateShowcase()
    {
        EditorWindow window = GetWindow<UIElementPlacerEditor>("UIElement Placer Editor");
    }

    public void OnEnable()
    {
    }

    public void OnGUI()
    {

        ShowMenu();

        if (GUILayout.Button("Create", GUILayout.Width(100f)))
        {
            if (canvas != null)
                CreateUIElement();
            else
                ShowNotification(new GUIContent("You need a Canvas to create the element !"));
        }

    }

    private void ShowMenu()
    {
        selectedPopUpPositionIndex = (UIElementPosition)EditorGUILayout.EnumPopup("Position", selectedPopUpPositionIndex);

        selectedPopUpTypeIndex = (UIElementType)EditorGUILayout.EnumPopup("Type", selectedPopUpTypeIndex);

        canvas = EditorGUILayout.ObjectField(canvas, typeof(Canvas), true) as Canvas;

        switch (selectedPopUpTypeIndex)
        {
            case UIElementType.LifeBar:
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Life Default Value (%) : ");
                    lifeDefaultValue = EditorGUILayout.FloatField(lifeDefaultValue);
                    if (lifeDefaultValue < 0)
                        lifeDefaultValue = 0;
                    else if (lifeDefaultValue > 100)
                        lifeDefaultValue = 100;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Life bar background image : ");
                    lifeBarImage = EditorGUILayout.ObjectField(lifeBarImage, typeof(Sprite), true) as Sprite;
                    GUILayout.EndHorizontal();
                }
                break;
            case UIElementType.MiniMap:
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Mini Map Camera : ");
                    miniMapCamera = EditorGUILayout.ObjectField(miniMapCamera, typeof(Camera), true) as Camera;
                    GUILayout.EndHorizontal();
                }
                break;
            case UIElementType.Portrait:
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Portrait Image : ");
                    portrait = EditorGUILayout.ObjectField(portrait, typeof(Sprite), true) as Sprite;
                    GUILayout.EndHorizontal();
                }
                break;
            default:
                break;
        }
    }

    private void CreateUIElement()
    {
        DefaultControls.Resources uiResources = new DefaultControls.Resources();
        GameObject uiElement = null;
        RectTransform rt = new RectTransform();
        switch (selectedPopUpTypeIndex)
        {
            case UIElementType.LifeBar:
                CreateLifeBar(ref uiElement, ref rt, uiResources);
                break;
            case UIElementType.MiniMap:
                CreateMiniMap(ref uiElement, ref rt, uiResources);
                break;
            case UIElementType.Portrait:
                CreatePortrait(ref uiElement, ref rt, uiResources);
                break;
            default:
                break;
        }
        if (rt != null)
        {
            SetAnchorsValues();
            SetAnchors(ref rt);
        }
    }

    private void CreateLifeBar(ref GameObject uiElement, ref RectTransform rt, DefaultControls.Resources uiResources)
    {
        if (lifeBarImage != null)
        {
            uiElement = DefaultControls.CreateImage(uiResources);
            uiElement.name = "LifeBar";
            Image lifebarImage = uiElement.GetComponent<Image>();
            lifebarImage.sprite = lifeBarImage;
            lifebarImage.type = Image.Type.Filled;
            rt = lifebarImage.rectTransform;
            float fillAmount = lifeDefaultValue / 100;
            lifebarImage.fillAmount = fillAmount;
            lifebarImage.fillMethod = Image.FillMethod.Horizontal;

            PutUIElementInCanvas(ref uiElement);
        }
        else
        {
            ShowNotification(new GUIContent("You need a background image to create the element !"));
        }
    }

    private void CreateMiniMap(ref GameObject uiElement, ref RectTransform rt, DefaultControls.Resources uiResources)
    {
        if (miniMapCamera != null)
        {
            uiElement = DefaultControls.CreateRawImage(uiResources);
            uiElement.name = "Minimap";

            RenderTexture renderTexture = new RenderTexture(1024, 1024, 0);
            renderTexture.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            renderTexture.filterMode = FilterMode.Trilinear;

            //Material mat = new Material(Shader.Find("UI/Default"));

            miniMapCamera.targetTexture = renderTexture;
            RawImage miniMapImage = uiElement.GetComponent<RawImage>();
            miniMapImage.texture = renderTexture;
            rt = miniMapImage.rectTransform;
            PutUIElementInCanvas(ref uiElement);

        }
        else
        {
            ShowNotification(new GUIContent("You need a Camera to create the element !"));
        }
    }

    private void CreatePortrait(ref GameObject uiElement, ref RectTransform rt, DefaultControls.Resources uiResources)
    {
        if (portrait != null)
        {
            uiElement = DefaultControls.CreateImage(uiResources);
            uiElement.name = "Portrait";
            Image portraitImage = uiElement.GetComponent<Image>();
            portraitImage.sprite = portrait;
            rt = portraitImage.rectTransform;
            PutUIElementInCanvas(ref uiElement);
        }
        else
        {
            ShowNotification(new GUIContent("You need an image to create the element !"));
        }
    }

    private Vector4 GetAnchor()
    {
        Vector4 toRet = new Vector4();
        Vector2 anchorMin = new Vector2();
        Vector2 anchorMax = new Vector2();
        switch (selectedPopUpPositionIndex)
        {
            case UIElementPosition.TopLeft:
                anchorMin = TopLeftPositionMin;
                anchorMax = TopLeftPositionMax;
                break;
            case UIElementPosition.TopRight:
                anchorMin = TopRightPositionMin;
                anchorMax = TopRightPositionMax;
                break;
            case UIElementPosition.BottomLeft:
                anchorMin = BottomLeftPositionMin;
                anchorMax = BottomLeftPositionMax;
                break;
            case UIElementPosition.BottomRight:
                anchorMin = BottomRightPositionMin;
                anchorMax = BottomRightPositionMax;
                break;
            default:
                break;
        }

        toRet = toRet.Set(anchorMin, anchorMax);
        return toRet;
    }

    private void SetAnchors(ref RectTransform rt)
    {
        Vector2[] anchors = GetAnchor().V4toTwoV2();
        rt.Set(anchors[0], anchors[1], Pivot, Offset);
    }

    private void SetAnchorsValues()
    {
        switch (selectedPopUpTypeIndex)
        {
            case UIElementType.LifeBar:
                SetLifeBarAnchors();
                break;
            case UIElementType.MiniMap:
                SetMiniMapAnchors();
                break;
            case UIElementType.Portrait:
                SetPortraitAnchors();
                break;
            default:
                break;
        }
    }

    private void SetLifeBarAnchors()
    {
        TopLeftPositionMin = UIAnchorsHelper.TopLeftPositionMinLife;
        TopRightPositionMin = UIAnchorsHelper.TopRightPositionMinLife;
        BottomLeftPositionMin = UIAnchorsHelper.BottomLeftPositionMinLife;
        BottomRightPositionMin = UIAnchorsHelper.BottomRightPositionMinLife;
        TopLeftPositionMax = UIAnchorsHelper.TopLeftPositionMaxLife;
        TopRightPositionMax = UIAnchorsHelper.TopRightPositionMaxLife;
        BottomLeftPositionMax = UIAnchorsHelper.BottomLeftPositionMaxLife;
        BottomRightPositionMax = UIAnchorsHelper.BottomRightPositionMaxLife;
    }

    private void SetMiniMapAnchors()
    {
        TopLeftPositionMin = UIAnchorsHelper.TopLeftPositionMinMiniMap;
        TopRightPositionMin = UIAnchorsHelper.TopRightPositionMinMiniMap;
        BottomLeftPositionMin = UIAnchorsHelper.BottomLeftPositionMinMiniMap;
        BottomRightPositionMin = UIAnchorsHelper.BottomRightPositionMinMiniMap;
        TopLeftPositionMax = UIAnchorsHelper.TopLeftPositionMaxMiniMap;
        TopRightPositionMax = UIAnchorsHelper.TopRightPositionMaxMiniMap;
        BottomLeftPositionMax = UIAnchorsHelper.BottomLeftPositionMaxMiniMap;
        BottomRightPositionMax = UIAnchorsHelper.BottomRightPositionMaxMiniMap;
    }

    private void SetPortraitAnchors()
    {
        TopLeftPositionMin = UIAnchorsHelper.TopLeftPositionMinPortrait;
        TopRightPositionMin = UIAnchorsHelper.TopRightPositionMinPortrait;
        BottomLeftPositionMin = UIAnchorsHelper.BottomLeftPositionMinPortrait;
        BottomRightPositionMin = UIAnchorsHelper.BottomRightPositionMinPortrait;
        TopLeftPositionMax = UIAnchorsHelper.TopLeftPositionMaxPortrait;
        TopRightPositionMax = UIAnchorsHelper.TopRightPositionMaxPortrait;
        BottomLeftPositionMax = UIAnchorsHelper.BottomLeftPositionMaxPortrait;
        BottomRightPositionMax = UIAnchorsHelper.BottomRightPositionMaxPortrait;
    }

    private void PutUIElementInCanvas(ref GameObject uiElement)
    {
        uiElement.transform.parent = canvas.transform;
    }
}
