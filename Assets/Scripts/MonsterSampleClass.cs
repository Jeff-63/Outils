using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterSampleClass : MonoBehaviour
{
    int selectedMonsterType;
    string[] monsterTypes;
    List<Monster> l;

    // Use this for initialization
    void Start()
    {
        monsterTypes = GenericClassCreator.GetAllFileNamesInFolder(MonsterFactory.monsterScriptPath, "cs", false);
        l = new List<Monster>();
        MonsterFactory.Instance.PreloadObjects(5);
        StartCoroutine(StartCleaningPool(5));
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnGUI()
    {

        GUI.Label(new Rect(0, 0, 100, 20), "Monster Type : ");
        selectedMonsterType = GUI.SelectionGrid(new Rect(100, 0, 800, 20), selectedMonsterType, monsterTypes, monsterTypes.Length);

        if (GUI.Button(new Rect(0, 40, 100, 50), "Create"))
        {
            Create();
        }
    }

    public void Create()
    {
        l.Add(MonsterFactory.Instance.CreateMonster(monsterTypes[selectedMonsterType]));
    }

    private IEnumerator StartCleaningPool(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            ObjectPool.Instance.CleanPool();
        }
    }

}
