using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimV2
{
    public AnimationCurve animCurve;
    public Vector2 animBounds;      //The range of y

    public float GetValue(float x)
    {
        return Mathf.Lerp(animBounds.x, animBounds.y, animCurve.Evaluate(Mathf.Clamp01(x)));
    }
}
