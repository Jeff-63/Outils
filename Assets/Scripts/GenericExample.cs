using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenericExample : MonoBehaviour {

    public delegate void ParameterlessDelegate();
    public delegate void StringArgDelegate(string s);
    // Use this for initialization
    void Start() {

        List<string> someStrings = new List<string>() { "a", "b", "c" };

        StringArgDelegate pd = delegate (string s) { Debug.Log(s); };
        //someStrings.Where(s => s.Contains("a"));

    }

    public void DoSomething()
    {

    }

    public void DebugAString(string toOut)
    {
        Debug.Log(toOut);
    }

   public class DelgPair
    {
        public StringArgDelegate delg;
        public string arg;

        public DelgPair(StringArgDelegate _delg, string _arg)
        {
            delg = _delg;
            arg = _arg;
        }

        public void InvokeDelg()
        {
            delg.Invoke(arg);
        }
    }


}
