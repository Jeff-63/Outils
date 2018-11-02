using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factorial : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log(FactorialFunction_Recursion(5));

    }

    public int FactorialFunction_NonRecursion(int fvalue)
    {
        int result = 0;
        for (int i = fvalue; i >= 0; i--)
        {
            result *= i;
        }

        return result;
    }

    public int FactorialFunction_Recursion(int fvalue)
    {
        if (fvalue == 1)
            return 1;
        else
            return fvalue * FactorialFunction_Recursion(fvalue - 1);
    }
}
