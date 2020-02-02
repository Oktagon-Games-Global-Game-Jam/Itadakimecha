using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public struct C_LadderComponentData : IComponentData
{
    public float2 x;
    public float2 y;
}

public struct TC_IsInLadderArea : IComponentData {}
