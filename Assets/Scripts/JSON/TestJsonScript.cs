using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class TestJsonScript : MonoBehaviour {
    //XML Examples: http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer
    //Binary Formater


    //https://docs.unity3d.com/Manual/JSONSerialization.html
    // Use this for initialization
    void Start ()
    {
        Dictionary<string, string> shinanigins = new Dictionary<string, string>()
        {
            {"salt","3" },
            {"pepper","nine" },
            {"Snakes","Pants" }
        };
        int meowTheThird = 4;

        JsonManager.SaveObjectAsJSONToStreamingAsset(shinanigins, "test123");
        JsonManager.SaveObjectAsJSONToStreamingAsset(meowTheThird, "Tradegy of darthwise the plague");

        Dictionary<string, string> unjsoned = JsonManager.LoadJsonFromStreamingAssetFile<Dictionary<string, string>>("test123.json");
        int meowTheFourth = JsonManager.LoadJsonFromStreamingAssetFile<int>("Tradegy of darthwise the plague.json");

        JSONNode jnode = JsonManager.LoadJSONNodeFromStreamingAssetFile("test123.json");

        ToJsonify tj = new ToJsonify();
        SomeMiniClass mc = new SomeMiniClass(10, 20, new List<string>() { "abba", "baba", "baab" }, "haha", "noooo", "hehe");
        SomeMiniClass mc2 = new SomeMiniClass(20, 30, new List<string>() { "cddc", "cdcdc", "dccd" }, "reee", "re", "reeeeeee");
        tj.miniClass = mc2;

        //JsonManager.SaveObjectAsJSONToStreamingAsset(tj, "tj");
        JsonManager.SaveObjectAsJSONToStreamingAsset(mc, "mc");
        
        //ToJsonify tjLoaded = JsonManager.LoadJsonFromStreamingAssetFile<ToJsonify>("tj.json");
        SomeMiniClass mc2Loaded = JsonManager.LoadJsonFromStreamingAssetFile<SomeMiniClass>("mc.json");
        string s = "reeeeeee";

        
        string j = JsonUtility.ToJson(tj);
        tj.someInt = 9999;
        JsonUtility.FromJsonOverwrite(j, tj);
        //ToJsonify tj2 = JsonUtility.FromJson<ToJsonify>(j);
        string ss = "reeeeeeeeeeeeeee";

    }



    public class SomeMiniClass
    {
        public int a, b;
        public List<string> shooka;
        public string[] arrayOfStrings;
        public SomeMiniClass(int _a, int _b, List<string> _shooka, params string[] _arrayOfStrings)
        {
            a = _a;
            b = _b;
            shooka = _shooka;
            arrayOfStrings = _arrayOfStrings;
        }
    }
	
}
