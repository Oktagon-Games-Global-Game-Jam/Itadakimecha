﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct C_SetPositionComponentData : IComponentData
{
    public float3 Position;
    public int DirectionX;
}
