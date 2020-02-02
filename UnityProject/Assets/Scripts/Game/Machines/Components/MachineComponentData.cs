using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct C_MachineComponentData : IComponentData
{
    public ProductType Input;
    public ProductType Result;
    public int IronQnty;
    public int CopperQnty;
    public int TimeToComplete;
}

public struct TC_Interact : IComponentData {}



public enum ProductType
{
    None = 0,
    Head,
    HotBody,
    Body,
    HotLeg,
    Leg
}
