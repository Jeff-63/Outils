using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuple<T, G> {
    public T obj1;
    public G obj2;

    public Tuple(T _obj1, G _obj2)
    {
        obj1 = _obj1;
        obj2 = _obj2;
    }

}
