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
    public bool freezeGravity;
    public float2 min;
    public float2 max;
}

public struct TC_CanClimbLadder : IComponentData {}

public struct C_LadderMovement : IComponentData
{
    public float speed;
}
