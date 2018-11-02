using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanificationSoftware : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        int minClasses = 2;
        List<Classe> classes;
        int slots = 4;
        string[] schedule = new string[slots];
        List<List<string>> al;

        List<string> planning = new List<string>();

        List<int> classeSlots = new List<int>() { 1, 2, 3 };
        Classe classeA = new Classe("A", classeSlots);

        classeSlots = new List<int>() { 3, 4 };
        Classe classeB = new Classe("B", classeSlots);

        classeSlots = new List<int>() { 2, 3 };
        Classe classeC = new Classe("C", classeSlots);

        classeSlots = new List<int>() { 1, 2, 4 };
        Classe classeD = new Classe("D", classeSlots);

        classes = new List<Classe>() { classeA, classeB, classeC, classeD };


        al = Initialize(slots, classes, schedule);

        Planning(al, schedule, 1, slots, planning, minClasses);
        ShowPlanning(planning);

    }


    public List<List<string>> Initialize(int slots, List<Classe> classes, string[] schedule)
    {
        List<List<string>> planning = new List<List<string>>();

        for (int i = 0; i < slots; i++)
        {
            planning.Add(new List<string>());
        }

        foreach (Classe c in classes)
        {
            foreach (int slot in c.timeSlots)
            {
                planning[slot - 1].Add(c.name);
            }
        }

        foreach (List<string> item in planning)
        {
            item.Add("0");
        }

        for (int i = 0; i < schedule.Length; i++)
        {
            schedule[i] = "0";
        }

        return planning;
    }

    public void Planning(List<List<string>> al, string[] schedule, int t, int tmax, List<string> planning, int minClasses)
    {

        foreach (string c in al[t - 1])
        {
            if (!Contains(schedule, c))
            {
                schedule[t - 1] = c;
            }

            if (t < tmax)
                Planning(al, schedule, t + 1, tmax, planning, minClasses);
            else if (isValid(schedule, minClasses))
            {
                string result = "[";
                foreach (string cl in schedule)
                {
                    result += " " + cl + " ";
                }
                result += "]";
                if (!planning.Contains(result))
                    planning.Add(result);
            }
            schedule[t - 1] = "0";
        }
    }

    public void ShowPlanning(List<string> planning)
    {
        foreach (string item in planning)
        {
            Debug.Log(item);
        }
    }

    public void ShowGrid(List<List<string>> al)
    {
        foreach (List<string> ls in al)
        {
            string line = "";
            foreach (string s in ls)
            {
                line += s;
            }
            Debug.Log(line);
            line = "";
        }
    }

    public bool Contains(string[] schedule, string c)
    {
        for (int i = 0; i < schedule.Length; i++)
        {
            if (schedule[i] == null)
                return false;
            else if (schedule[i].Contains(c))
                return true;
        }

        return false;
    }

    public bool isValid(string[] schedule, int minClasses)
    {
        int cpt = 0;
        for (int i = 0; i < schedule.Length; i++)
        {
            if (schedule[i] != null && !schedule[i].Contains("0"))
                cpt++;
        }

        return cpt >= minClasses;
    }

    public class Classe
    {
        public string name;
        public List<int> timeSlots;

        public Classe(string _name, List<int> _timeSlots)
        {
            name = _name;
            timeSlots = _timeSlots;
        }


    }

}
