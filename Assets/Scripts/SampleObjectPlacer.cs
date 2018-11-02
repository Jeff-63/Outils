using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SampleObjectPlacer : MonoBehaviour
{
    public enum toto { a,b,c,d};
    public ObjectPlacer objectPlacer;

    private void Start()
    {
        int[] arr = new int[] { 1, 2, 3, 2, 1 }; //inline constructor

        /*Debug.Log("count :" + arr.Count((i) => { return i > 3; }));
        arr = arr.Where((i) => { return i > 3; }).ToArray();

        Vector5 v5 = new Vector5(1, 2, 3, 4, 5);
        Vector5.dlg d = (float i) => { return i > 3; };*/

        /*Debug.Log(v5.Exist(d));*/
        Debug.Log(arr.Compiles((int i) => { return i > 0; }));

        /*float ang = 45;
        Vector2 result = ang.AngToV2();

        int f = 5;
        int res = f.Factorial();

        string test = arr.ArrToString();*/

        Vector2 v = new Vector2(90,45);
        Debug.Log(v.V2ToAngle());

    }
}
