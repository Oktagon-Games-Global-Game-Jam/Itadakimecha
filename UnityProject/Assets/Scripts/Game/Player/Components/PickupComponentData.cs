using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct TC_CanPick : IComponentData {}

public struct C_PickInfo : IComponentData
{
    public int PickupDistance;
}
public struct TC_Pickable : IComponentData {}

public struct TC_PickHoldAction : IComponentData {}

public struct TC_InHold : IComponentData {}

public struct MC_RemoveInHold : IComponentData
{
    public  float3 Position;
}

// This will hold the picked Entity
public struct C_HoldComponentData : IComponentData
{
    public Entity Item;
}

public struct TC_Dropping : IComponentData {}
