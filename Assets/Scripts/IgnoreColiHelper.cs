using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColiHelper
{

    #region Singleton
    private static IgnoreColiHelper instance;

    private IgnoreColiHelper() { }

    public static IgnoreColiHelper Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new IgnoreColiHelper();
            }
            return instance;
        }
    }
    #endregion

    List<IgnoreColiHelperSlot> ignoreList = new List<IgnoreColiHelperSlot>();

    public void Update(float dt)
    {
        for (int i = ignoreList.Count - 1; i >= 0; i--)
        {
            ignoreList[i].timer -= dt;
            if (ignoreList[i].timer <= 0)
            {
                AddIgnore(ignoreList[i].coli1, ignoreList[i].coli2, ignoreList[i].setIgnore);
                ignoreList.RemoveAt(i);
            }
        }
    }

    public void AddInitialTimedIgnore(Collider2D[] coliGroup1, Collider2D[] coliGroup2, bool instantSet, float timer, bool afterTimeSet)
    {
        foreach (Collider2D coli1 in coliGroup1)
            foreach (Collider2D coli2 in coliGroup2)
            {
                AddIgnore(coli1, coli2, instantSet);
                AddTimedIgnore(coli1, coli2, timer, afterTimeSet);
            }
    }

    public void AddInitialTimedIgnore(Collider2D[] coliGroup1, Collider2D coli2, bool instantSet, float timer, bool afterTimeSet)
    {
        foreach (Collider2D coli1 in coliGroup1)
        {
            AddIgnore(coli1, coli2, instantSet);
            AddTimedIgnore(coli1, coli2, timer, afterTimeSet);
        }
    }

    /// <summary>
    /// Sets ignore between coli 1 & 2 instantly to 'instantSet', after timer, sets to afterTimeSet
    /// </summary>
    public void AddInitialTimedIgnore(UnityEngine.Collider2D coli, UnityEngine.Collider2D coli2, bool instantSet, float timer, bool afterTimeSet)
    {
        AddIgnore(coli, coli2, instantSet);
        AddTimedIgnore(coli, coli2, timer, afterTimeSet);
    }

    public void AddTimedIgnore(UnityEngine.Collider2D coli, UnityEngine.Collider2D coli2, float timer, bool setIgnore)
    {
        ignoreList.Add(new IgnoreColiHelperSlot(coli, coli2, timer, setIgnore));
    }

    public void AddIgnore(UnityEngine.Collider2D coli, UnityEngine.Collider2D coli2, bool setIgnore)
    {
        if (coli && coli2)
            Physics2D.IgnoreCollision(coli, coli2, setIgnore);
    }

    private class IgnoreColiHelperSlot
    {
        public UnityEngine.Collider2D coli1, coli2;
        public float timer;
        public bool setIgnore;

        public IgnoreColiHelperSlot(UnityEngine.Collider2D _coli1, UnityEngine.Collider2D _coli2, float _timer, bool _setIgnore)
        {
            coli1 = _coli1;
            coli2 = _coli2;
            timer = _timer;
        }

    }

}