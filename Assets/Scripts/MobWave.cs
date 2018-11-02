using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MobWave
{
    public enum WaveType
    {
        Mobs,
        Boss
    }

    public WaveType Type;
    public GameObject Prefab;
    public int Count;
}
