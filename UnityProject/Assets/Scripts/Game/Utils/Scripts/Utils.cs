using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Utils
{
    public static bool IsInRange(float position, float x1, float x2)
    {
        return (position >= x1) && position <= x2;
    }

    public static float CalculateDistance(float x1, float x2)
    {
        return math.distance(x1, x2);
    }

}
