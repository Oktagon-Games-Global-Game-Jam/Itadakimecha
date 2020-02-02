using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;
using UnityEngine;
using System.Collections.Generic;

public class S_MovementInput : ComponentSystem
{
    private EntityQuery m_EntityQuery;

    protected override void OnCreate()
    {
        m_EntityQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { ComponentType.ReadOnly<PlayerInput_C>(), typeof(MovementComponentData) },
        });
    }
       
    protected override void OnUpdate()
    {
        var entities = m_EntityQuery.ToEntityArray(Allocator.TempJob);
        var playerInput = m_EntityQuery.ToComponentDataArray<PlayerInput_C>(Allocator.TempJob);
        var movement = m_EntityQuery.ToComponentDataArray<MovementComponentData>(Allocator.TempJob);

        for (int i = 0; i < playerInput.Length; i++)
        {
            float horizontal = Input.GetAxis($"Horizontal_{playerInput[i].inputId}");
            EntityManager.AddComponentData(entities[i], new TC_MovingComponentData { Value = horizontal });
        }

        entities.Dispose();
        playerInput.Dispose();
        movement.Dispose();
    }
}

public class S_PickupInput : ComponentSystem
{
    private EntityQuery m_EntityQuery;

    protected override void OnCreate()
    {
        m_EntityQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { ComponentType.ReadOnly<PlayerInput_C>(), ComponentType.ReadOnly<C_CanPick>() },
        });
    }

    protected override void OnUpdate()
    {
        var entities = m_EntityQuery.ToEntityArray(Allocator.TempJob);
        var playerInput = m_EntityQuery.ToComponentDataArray<PlayerInput_C>(Allocator.TempJob);

        for (int i = 0; i < playerInput.Length; i++)
        {
            if(Input.GetButton($"Action_{playerInput[i].inputId}"))
            {
                EntityManager.AddComponent<TC_PickHoldAction>(entities[i]);
            }
        }

        entities.Dispose();
        playerInput.Dispose();
    }
}