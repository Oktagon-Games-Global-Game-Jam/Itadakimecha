using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct C_ResourceComponentData : IComponentData
{
    public int IronResources;
    public int CopperResources;
}

public struct MC_ChangeResouceData : IComponentData
{
    public int DeltaIron;
    public int DeltaCopper;
}
