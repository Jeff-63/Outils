using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  //Required for MenuItem, means that this is an Editor script, must be placed in an Editor folder, and cannot be compiled!
using System.Linq;  //Used for Select

public class ColorWindow : EditorWindow
{ //Now is of type EditorWindow

    [MenuItem("Custom Tools/ Color Window")] //This the function below it as a menu item, which appears in the tool bar
    public static void CreateShowcase() //Menu items can call STATIC functions, does not work for non-static since Editor scripts cannot be attached to objects
    {
        EditorWindow window = GetWindow<ColorWindow>("Color Window");
    }

    private Color[] colors;
    private int width = 8;
    private int height = 8;
    Texture colorTexture;
    List<Renderer> textureTargets = new List<Renderer>();
    bool showRandomizeColorToggleGroup = false;

    Color selectedColor = Color.white;
    Color randomColor;
    GUIContent label = new GUIContent();
    int x, y;
    float randomRedValue, randomGreenValue, randomBlueValue;

    public void OnEnable()
    {
        Generate();
    }

    private Color GetRandomColor()  //Built a get random color tool
    {
        return new Color(Random.value, Random.value, Random.value, 1f);
    }

    void OnGUI() //Called every frame in Editor window
    {
        GUILayout.BeginHorizontal();        //Have each element below be side by side
        DoControls();
        DoCanvas();
        GUILayout.EndHorizontal();
    }

    void DoControls()
    {
        GUILayout.BeginVertical();                                                      //Start vertical section, all GUI draw code after this will belong to same vertical
        GUILayout.Label("ToolBar", EditorStyles.largeLabel);                            //A label that says "Toolbar"

        GUILayout.BeginHorizontal();
        x = EditorGUILayout.IntField("X : ", x);
        y = EditorGUILayout.IntField("Y : ", y);
        if (GUILayout.Button("b", GUILayout.Width(20f)))
            Regenerate();
        GUILayout.EndHorizontal();
        selectedColor = EditorGUILayout.ColorField("Paint Color", selectedColor);       //Make a color field with the text "Paint Color" and have it fill the selectedColor var
        if (GUILayout.Button("Fill All"))                                           //A button, if pressed, returns true
        {
            colors = colors.Select(c => c = selectedColor).ToArray();                   //Linq expresion, for every color in the color array, sets it to the selected color
        }
        DrawUILine();

        showRandomizeColorToggleGroup = EditorGUILayout.Toggle("Randomize Color", showRandomizeColorToggleGroup);
        if (showRandomizeColorToggleGroup)
        {
            label.text = "R";
            randomRedValue = EditorGUILayout.Slider(label, randomRedValue, 0, 1f);
            label.text = "G";
            randomGreenValue = EditorGUILayout.Slider(label, randomGreenValue, 0, 1f);
            label.text = "B";
            randomBlueValue = EditorGUILayout.Slider(label, randomBlueValue, 0, 1f);
        }

        GUILayout.FlexibleSpace();                                                      //Flexible space uses any left over space in the loadout
        /*textureTarget = EditorGUILayout.ObjectField("Output Renderer", textureTarget, typeof(Renderer), true) as Renderer;  //Build an object field that accepts a renderer*/

        for (int i = 0; i < textureTargets.Count; i++)
        {
            GUILayout.BeginHorizontal();                //Every GUI element created after this call will belong to the same horizontal line

            Renderer result = EditorGUILayout.ObjectField(GetRendererAt(i), typeof(Renderer), true) as Renderer; //Make a field that can be set to any object, and returns the object in it
            if (GUILayout.Button("-", GUILayout.Width(20f)))  //A button of width 20
                RemoveRendererAtIndex(i);
            GUILayout.EndHorizontal();                  //Ends the horizontal line called earlier
        }

        DropAreaGUI();

        if (GUILayout.Button("Save to Objects"))
        {
            foreach (Renderer r in textureTargets)
            {
                Texture2D t2d = new Texture2D(width, height);                               //Create a new texture
                t2d.filterMode = FilterMode.Point;                                          //Simplest non-blend texture mode
                r.material = new Material(Shader.Find("Diffuse"));              //Materials require Shaders as an arguement, Diffuse is the most basic type
                r.sharedMaterial.mainTexture = t2d;                             //sharedMaterial is the MAIN RESOURCE MATERIAL. Changing this will change ALL objects using it, .material will give you the local instance

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        int index = j + i * height;
                        t2d.SetPixel(i, height - 1 - j, colors[index]);                     //Color every pixel using our color table, the texture is 8x8 pixels large, but strecthes to fit
                    }
                }
                t2d.Apply();
            }
            textureTargets.Clear();
                                                                            //Apply all changes to texture
        }
        GUILayout.EndVertical();                                                        //end vertical section
    }

    void DoCanvas()
    {
        Event evt = Event.current;                     //Grab the current event

        Color oldColor = GUI.color;                    //GUI color uses a static var, need to save the original to reset it
        GUILayout.BeginHorizontal();                   //All following gui will be on one horizontal line until EndHorizontal is called
        for (int i = 0; i < width; i++)
        {
            GUILayout.BeginVertical();                //All following gui will be in a vertical line
            for (int j = 0; j < height; j++)
            {
                int index = j + i * height;           //Rememeber, this is just like a 2D array, but in 1D
                Rect colorRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)); //Reserve a square, which will autofit to the size given
                if ((evt.type == EventType.MouseDown || evt.type == EventType.MouseDrag) && colorRect.Contains(evt.mousePosition)) //Can now paint while dragging update
                {
                    if (evt.button == 0)//If mouse button pressed is left
                    {
                        randomColor = selectedColor;
                        colors[index] = ApplyRandomColor(randomColor);
                    }
                    else
                    {
                        selectedColor = colors[index];//Sample color
                    }

                    evt.Use();                        //The event was consumed, if you try to use event after this, it will be non-sensical
                }
                GUI.color = colors[index];            //Same as a 2D array
                GUI.DrawTexture(colorRect, colorTexture); //This is colored by GUI.Color!!!
            }
            GUILayout.EndVertical();                  //End Vertical Zone
        }
        GUILayout.EndHorizontal();                    //End horizontal zone
        GUI.color = oldColor;                         //Restore the old color
    }

    void Generate()
    {
        colors = new Color[width * height];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = GetRandomColor();
        colorTexture = EditorGUIUtility.whiteTexture;
    }

    void Regenerate()
    {
        if (x > 0 && y > 0)
        {
            width = x;
            height = y;
            Generate();
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

    Color ApplyRandomColor(Color color)
    {
        Color result = color;
        if (showRandomizeColorToggleGroup)
        {
            result.r += Random.Range(-randomRedValue, randomRedValue);
            result.g += Random.Range(-randomGreenValue, randomGreenValue);
            result.b += Random.Range(-randomBlueValue, randomBlueValue);
        }
        return result;
    }

    private void DropAreaGUI()
    {
        Event evt = Event.current;                                                              //Grab any event currently being processed
        Rect dropArea = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));         //Create a flexible Rect object
        GUI.Box(dropArea, "Add Renderers");                                                      //Creates a GUI element that is a box, that takes a rect as an arg
        switch (evt.type)                                //What is the event type?
        {

            case EventType.DragUpdated:                 //Every frame a drag is occuring
            case EventType.DragPerform:                 //The frame when a drag is finished
                if (!dropArea.Contains(evt.mousePosition))
                    break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy; //Sets the plus sign on the cursor, apparently is required for drag modes

                if (evt.type == EventType.DragPerform)   //Was frame it was released
                {
                    DragAndDrop.AcceptDrag();           //Lets the event system know that this controller has handled the drag event
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        GameObject go = draggedObject as GameObject; //Cast the dragged object, if it was not a gameobject (a different script type or resource) ignore it
                        if (!go)
                            continue;

                        Transform t = go.transform;     //In theory can never happen, but imagine it was a different script type
                        if (!t)
                            continue;
                        int childrens = t.childCount;
                        if (childrens > 0)
                        {
                            for (int i = 0; i < childrens; ++i)
                            {
                                if (t.GetChild(i).GetComponent<Renderer>())
                                    AddRenderers(t.GetChild(i).GetComponent<Renderer>());
                            }
                        }
                        else
                        {
                            if (t.GetComponent<Renderer>())
                                AddRenderers(t.GetComponent<Renderer>());
                        }
                    }

                }
                Event.current.Use();                    //States this event was used (So no other controllers use this event)
                break;
        }
    }

    public void AddRenderers(Renderer toAdd)
    {
        textureTargets.Add(toAdd);
    }

    public Renderer GetRendererAt(int index)
    {
        return textureTargets[index];
    }

    private void RemoveRendererAtIndex(int index)
    {
        textureTargets.RemoveAt(index);                                          
    }
}
