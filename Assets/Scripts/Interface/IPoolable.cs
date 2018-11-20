using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable{

    GameObject gameObject { get; }
    void Pool();
    void DePool();
    void ReInitialize();
}
