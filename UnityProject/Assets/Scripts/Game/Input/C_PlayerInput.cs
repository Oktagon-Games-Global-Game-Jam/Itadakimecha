using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PlayerInput_C : IComponentData
{
    public int inputId;
}