using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class TimerReflexion : MonoBehaviour
{
    void Start()
    {
        Sample s = new Sample();

        StartCoroutine(Timer.Instance.CreateTimer(s, "MyMethod", 1, null));
        StartCoroutine(Timer.Instance.CreateTimer(s, "MyMethod2", 1, null));
        StartCoroutine(Timer.Instance.CreateTimer(s, "MyMethod", 5, null));
    }


    public class Timer
    {
        #region Singleton
        private static Timer instance;

        public static Timer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Timer();
                }
                return instance;
            }
        }
        #endregion


        private Timer()
        {
            memberInfoLists = new Dictionary<Type, Dictionary<string, MemberInfo>>();
        }

        private Dictionary<Type, Dictionary<string, MemberInfo>> memberInfoLists;

        /// <summary>
        /// Call the method or property given 
        /// with or without parameters
        /// after n second
        /// </summary>
        /// <param name="toInvoke">The object who invoke the given methode or property</param>
        /// <param name="methodOrPropertyToCall"> The name of the method or property you want to call</param>
        /// <param name="timer">the time before the invoke</param>
        /// <param name="args"> the arg of the method or property (Can be null)</param>
        /// <returns></returns>
        /// 
        public IEnumerator CreateTimer(object toInvoke, string methodOrPropertyToCall, float timer, object[] args)
        {
            yield return new WaitForSeconds(timer);
            Type type = toInvoke.GetType();
            MemberInfo mi;

            if (!Exist(methodOrPropertyToCall, type))//not in the dictionnary
            {
                mi = type.GetMethod(methodOrPropertyToCall);
                if (mi == null)
                {
                    mi = type.GetProperty(methodOrPropertyToCall);
                    if (mi == null)
                    {
                        Debug.Log("Unknown method or property " + methodOrPropertyToCall + " for the class " + type);
                    }
                    else
                    {
                        AddMemberInfoToDictionnary(mi, methodOrPropertyToCall, type, toInvoke, args);
                    }
                }
                else
                {
                    AddMemberInfoToDictionnary(mi, methodOrPropertyToCall, type, toInvoke, args);
                }
            }
            else
            {
                Debug.Log("Existing method called");
                CallMemberInfo(GetMemberInfo(methodOrPropertyToCall, type), toInvoke, args);
            }

        }

        private bool Exist(string methodOrProperty, Type type)
        {
            if (memberInfoLists.ContainsKey(type))
            {
                foreach (KeyValuePair<string, MemberInfo> pair in memberInfoLists[type])
                {
                    if (pair.Key == methodOrProperty)
                        return true;
                }
            }

            return false;
        }

        private void CallMemberInfo(MemberInfo mi, object toInvoke, object[] args)
        {
            if (mi is MethodInfo)
            {
                try
                {
                    ((MethodInfo)mi).Invoke(toInvoke, args);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
            else if (mi is PropertyInfo)
            {
                //call property
                try
                {
                    ((PropertyInfo)mi).GetSetMethod().Invoke(toInvoke, args);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                }
            }
        }

        private void AddMemberInfoToDictionnary(MemberInfo mi, string methodOrPropertyToCall, Type type, object toInvoke, object[] args)
        {
            if (!memberInfoLists.ContainsKey(type))
            {
                Dictionary<string, MemberInfo> newArray = new Dictionary<string, MemberInfo>();
                memberInfoLists.Add(type, newArray);
            }
            memberInfoLists[type].Add(methodOrPropertyToCall, mi);
            Debug.Log("Method added !");
            CallMemberInfo(mi, toInvoke, args);
        }

        private MemberInfo GetMemberInfo(string methodOrPropertyToCall, Type type)
        {
            Dictionary<string, MemberInfo> list = memberInfoLists[type];
            MemberInfo mi =  list[methodOrPropertyToCall];
            return mi;
        }
    }
    public class Sample
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public void MyMethod()
        {
            Debug.Log("Hello from MyMethod");
        }

        public void MyMethod2()
        {
            Debug.Log("Hello from MyMethod2");
        }

        public void MyMethod3(int i)
        {
            Debug.Log("Hello from MyMethod3 : " + i);
        }

        public override string ToString()
        {
            return Name + " : " + Age + "ans";
        }
    }

}
