using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public struct C_LadderComponentData : IComponentData
{
    public float2 min;
    public float2 max;
}

public struct TC_IsInLadderArea : IComponentData {}

public struct TC_CanClimbLadder : IComponentData {}
