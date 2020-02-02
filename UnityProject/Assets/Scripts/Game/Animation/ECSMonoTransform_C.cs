using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct MonoTransform_C : IComponentData
{
    public int id;
}

public struct SyncMonoTransform_C : IComponentData
{
    public float3 position;
}