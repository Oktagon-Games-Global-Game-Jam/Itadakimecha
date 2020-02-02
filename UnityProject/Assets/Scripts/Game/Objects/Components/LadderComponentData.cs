using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public struct C_LadderComponentData : IComponentData
{
    public Entity entity;
    public float2 min;
    public float2 max;
}

public struct C_IsInLadderArea : IComponentData
{
    public float2 min;
    public float2 max;
}

public struct TC_CanClimbLadder : IComponentData {}
