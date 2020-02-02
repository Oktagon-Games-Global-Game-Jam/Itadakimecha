using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Utils
{
    public static bool IsInRange(float position, float x1, float x2)
    {
        if(x1 < x2)
            return (position >= x1) && position <= x2;
        else
            return (position <= x1) && position >= x2;
        
    }

    public static bool IsInside(float2 pointX, float2 widthHeightX,float2 pointY, float2 widthHeightY)
    {
        if (pointX.x > pointY.x + widthHeightY.x)
            return false;
        if (pointX.x + widthHeightX.x < pointY.x)
            return false;
        
        if (pointX.y > pointY.y + widthHeightY.y)
            return false;
        if (pointX.y + widthHeightX.y < pointY.y)
            return false;

        return true;
    }

    public static float CalculateDistance(float x1, float x2)
    {
        return math.distance(x1, x2);
    }

}
