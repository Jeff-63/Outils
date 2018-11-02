using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXAMPLE : MonoBehaviour
{

    public void Start()
    {
        int i = 5;
        Wrapper<int> wrappedInt = i.Wrap();
    }

    public class SomeClass
    {
        public float a, b, c, d, e, f, g, h, i;

    }


    public Animator itemStateMachine = new Animator();
    public void SetAnimValue(string toSet, object _value, bool isTrigger = false)
    {
        try
        {
            if (isTrigger)
            {
                if ((bool)_value == true)
                    itemStateMachine.SetTrigger(toSet);
                else
                    itemStateMachine.ResetTrigger(toSet);
            }
            else
            {
                switch (_value.GetType().ToString())
                {
                    case "Bool":
                        itemStateMachine.SetBool(toSet, (bool)_value);
                        break;
                    case "Float":
                        itemStateMachine.SetFloat(toSet, (float)_value);
                        break;
                    default:
                        Debug.Log("Unhandled switch case for: " + _value.GetType().ToString() + " SetAnimValue not called");
                        break;
                }
            }
        }
        catch
        {
            Debug.Log("Error, failure inside SetAnimValue for value: " + toSet + " for value type: " + _value.GetType() + ", isTrigger: " + isTrigger);
            //If you see this, you probably sent the wrong type as the object to the parameter toSet, for exmple, KeyPressed is a trigger, not a bool, or Ammo is a float, not a bool
        }
    }
}
