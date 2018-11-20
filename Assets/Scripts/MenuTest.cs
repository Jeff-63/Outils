using UnityEditor;
using UnityEngine;
public class MenuTest : MonoBehaviour
{
    /*
    You can assign hotkeys to menu items,
    To create a hotkey you can use the following special characters:
    % (ctrl on Windows, cmd on macOS), 
    # (shift), 
    & (alt). 
    If no special modifier key combinations are required the key can be given after an underscore. 
        For example to create a menu with hotkey shift-alt-g use "MyMenu/Do Something #&g". 
        To create a menu with hotkey g and no key modifiers pressed use "MyMenu/Do Something _g".

    A hotkey text must be preceded with a space character
        ("MyMenu/Do_g" won't be interpreted as hotkey, while "MyMenu/Do _g" will).

    */

    // Add a menu item named "Do Something" to MyMenu in the menu bar.
    [MenuItem("MyMenu/Do Something")]
    static void DoSomething()
    {
        Debug.Log("Doing Something...");
    }

    // Validated menu item.
    // Add a menu item named "Log Selected Transform Name" to MyMenu in the menu bar.
    // We use a second function to validate the menu item
    // so it will only be enabled if we have a transform selected.
    [MenuItem("MyMenu/Log Selected Transform Name")]
    static void LogSelectedTransformName()
    {
        Debug.Log("Selected Transform is on " + Selection.activeTransform.gameObject.name + ".");
    }

    // Validate the menu item defined by the function above.
    // The menu item will be disabled if this function returns false.
    [MenuItem("MyMenu/Log Selected Transform Name", true)]
    static bool ValidateLogSelectedTransformName()
    {
        // Return false if no transform is selected.
        return Selection.activeTransform != null;
    }

    // Add a menu item named "Do Something with a Shortcut Key" to MyMenu in the menu bar
    // and give it a shortcut (ctrl-g on Windows, cmd-g on macOS).
    [MenuItem("MyMenu/Open Test Window %g")]
    static void DoSomethingWithAShortcutKey()
    {
        EditorWindow.GetWindow<EditorWindowTest>();        
    }

    // Add a menu item called "Double Mass" to a Rigidbody's context menu.
    //The context menu is visible when right clicking on an object
    [MenuItem("CONTEXT/Rigidbody/Double Mass")]
    static void DoubleMass(MenuCommand command)
    {
        Rigidbody body = (Rigidbody)command.context;
        body.mass = body.mass * 2;
        Debug.Log("Doubled Rigidbody's Mass to " + body.mass + " from Context Menu.");
    }

    [MenuItem("CONTEXT/Flammable/I am fire")]
    static void FlammableThing(MenuCommand command)
    {
        Debug.Log("Burn baby burn");
    }

    // Add a menu item to create custom GameObjects.
    // Priority 1 ensures it is grouped with the other menu items of the same kind
    // and propagated to the hierarchy dropdown and hierarch context menus.
    //Each menu item is grouped in increments of 50 (Small line will divide them)
    [MenuItem("GameObject/MyCategory/Custom Game Object", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Custom Game Object");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
    public static bool ShowSection = false;
    [PreferenceItem("New Preference item")]     //Appears as a menu item in Preferences
    static void CreatePreferenceMenu()
    {
        
        //Use EditorGUILAyout
        ShowSection = EditorGUILayout.BeginToggleGroup("Show Section", ShowSection);   //Toggle button that starts a toggle group
        if (GUILayout.Button("Hi", GUILayout.Width(20f)))                                   //Not visible while toggled off
            Debug.Log("Hello");
        EditorGUILayout.EndToggleGroup();                                                   //End of toggle group

        //EditorPrefs.SetBool   ||  EditorPrefs.HasKey                                      //Same as player prefs, but for editor         
    }

    //Other kinds of Menus
    //PopUp windows - These lose focus when they are unselected
    //https://docs.unity3d.com/ScriptReference/PopupWindow.html

    //See the Wizard script for wizard menu example



}