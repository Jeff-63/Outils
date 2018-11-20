using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool
{
    #region Singleton
    private static ObjectPool instance;

    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }
    #endregion

    public GameObject pool;
    Dictionary<Type, Stack<IPoolable>> iPoolablePool;
    Dictionary<string, GameObject> goPool;

    private ObjectPool()
    {
        Initialize();
    }

    private void Initialize()
    {
        iPoolablePool = new Dictionary<Type, Stack<IPoolable>>();
        goPool = new Dictionary<string, GameObject>();
        pool = GameObject.Find("Pool");
        if (!pool)
            pool = new GameObject();
        pool.name = "Pool";
        pool.SetActive(false);
    }

    public IPoolable GetFromPool(Type type)
    {
        if (iPoolablePool.ContainsKey(type))
        {
            if (iPoolablePool[type].Count > 0)
            {
                IPoolable toRet = iPoolablePool[type].Pop();
                toRet.gameObject.transform.parent = null;
                return toRet;
            }
            else
                return null;
        }
        else
            return null;
    }

    public void PutInPool(IPoolable iPoolableObject, Type type, string name)
    {
        iPoolableObject.gameObject.transform.parent = GetIPoolableParent(iPoolableObject, name).transform;
        if (iPoolablePool.ContainsKey(type))
        {
            iPoolablePool[type].Push(iPoolableObject);
        }
        else
        {
            Stack<IPoolable> newStackIPoolable = new Stack<IPoolable>();
            newStackIPoolable.Push(iPoolableObject);
            iPoolablePool.Add(type, newStackIPoolable);
        }
    }

    private GameObject GetIPoolableParent(IPoolable iPoolableObject, string objectName)
    {
        if (goPool.ContainsKey(objectName))
        {
            return goPool[objectName];
        }

        GameObject parent;
        parent = new GameObject();
        parent.name = objectName + "s";
        parent.transform.parent = pool.transform;
        goPool.Add(objectName, parent);
        return parent;
    }

    public void CleanPool()
    {
        foreach (KeyValuePair<Type, Stack<IPoolable>> pair in iPoolablePool)
        {
            if (pair.Value.Count > 0)
            {
                for (int i = 0; i <= pair.Value.Count / 2; i++)
                {
                    GameObject.Destroy(pair.Value.Pop().gameObject);
                }
            }
        }
    }
}
