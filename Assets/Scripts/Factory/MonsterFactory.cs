using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterFactory
{

    #region Singleton
    private static MonsterFactory instance;

    public static MonsterFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MonsterFactory();
            }
            return instance;
        }
    }
    #endregion

    public static string monsterScriptPath = Application.dataPath + "/Scripts/Monsters/MonsterChildrens/";
    string monsterPrefabPath = "Prefabs/Monsters/";

    Dictionary<string, Type> monsterTypeList;
    Dictionary<Type, GameObject> monsterList;

    private MonsterFactory()
    {
        Initialize();
    }

    private void Initialize()
    {
        monsterTypeList = GenericClassCreator.GetAllTypesFromFileLocation(monsterScriptPath);
        monsterList = SetMonsterList();
    }

    public Monster CreateMonster(string monsterName, Vector3 location)
    {
        if (!monsterTypeList.ContainsKey(monsterName))
        {
            Debug.LogError("Monster : " + monsterName + " not found");
            return null;
        }

        GameObject go = null;
        Monster monster = (Monster)ObjectPool.Instance.GetFromPool(monsterTypeList[monsterName]);

        if (monster != null)
        {
            monster.ReInitialize();
            go = monster.gameObject;
        }
        else
        {
            go = GameObject.Instantiate(monsterList[monsterTypeList[monsterName]]);
        }

        go.name = go.name.Replace("(Clone)", "");
        go.transform.position = location;

        return go.GetComponent<Monster>();
    }

    public Monster CreateMonster(string monsterName)
    {
        Vector3 randomLocation = GetRandomLocation();
        return CreateMonster(monsterName, randomLocation);
    }

    public void PutMonsterInThePool(Monster monster)
    {
        ObjectPool.Instance.PutInPool(monster, monster.GetType(), monster.name);
    }

    public Dictionary<Type, GameObject> SetMonsterList()
    {
        Dictionary<Type, GameObject> toReturn = new Dictionary<Type, GameObject>();
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>(monsterPrefabPath);

        foreach (GameObject go in allPrefabs)
        {
            Type t = monsterTypeList[go.name];
            toReturn.Add(t, go);
        }

        return toReturn;
    }

    public Vector3 GetRandomLocation()
    {
        return new Vector3(UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(0, 20), UnityEngine.Random.Range(-20, 20));
    }

    public void PreloadObjects(int amount)
    {
        string[] monsterList = GenericClassCreator.GetAllFileNamesInFolder(monsterScriptPath, "cs", false);
        for (int i = 0; i < amount; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, monsterTypeList.Count - 1);
            Type objectType = monsterTypeList[monsterList[randIndex]];
            if (objectType != typeof(Monster))
            {
                Monster m = CreateMonster(objectType.ToString());
                PutMonsterInThePool(m);
            }
        }
    }

}
