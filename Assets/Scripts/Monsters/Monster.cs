using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IPoolable
{

    public void DePool()
    {
        ReInitialize();
    }

    public void Pool()
    {
       //stop les composants en cours de fonctionnement du gameobject lié au script
    }

    public void OnMouseDown()
    {
        Delete();
    }

    public void Delete()
    {
        MonsterFactory.Instance.PutMonsterInThePool(this);
    }

    public void ReInitialize()
    {
        // réinitialise les données et les composants du GameObject
    }
}
