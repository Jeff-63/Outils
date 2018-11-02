using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestEnum { ValueOne, ValueTwo, ValueThree };

[System.Serializable]
public class ObjectPlacer
{
    public TestEnum testEnum = TestEnum.ValueTwo;
}
