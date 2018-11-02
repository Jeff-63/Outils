using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RectBuilder
{
    public enum ScalableType { ConstantWidth, PercentageWidth }

    static readonly int CELL_WIDTH = 15;
    static readonly int CELL_HEIGHT = 15;

    public static Rect GetRect(int x, int y, int width, int height, Vector2 offset, ScalableType scalability)
    {
        Rect result = new Rect();

        result.y = offset.y;
        result.x = offset.x;

        result.y += y * CELL_HEIGHT;
        result.x += x * CELL_WIDTH; ;

        switch (scalability)
        {
            case ScalableType.ConstantWidth:
                result.width = width * CELL_WIDTH;
                result.height = height * CELL_HEIGHT;
                break;
            case ScalableType.PercentageWidth:
                break;
            default:
                break;
        }
        
        return result;
    }
}
