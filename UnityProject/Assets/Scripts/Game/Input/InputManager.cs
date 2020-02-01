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
            All = new ComponentType[] { ComponentType.ReadOnly<C_PlayerInput>(), typeof(TC_CanMove) },
        });
    }

    protected override void OnUpdate()
    {
        var entities = m_EntityQuery.ToEntityArray(Allocator.TempJob);
        var playerInput = m_EntityQuery.ToComponentDataArray<C_PlayerInput>(Allocator.TempJob);

        for (int i = 0; i < playerInput.Length; i++)
        {
            float horizontal = Input.GetAxis($"Horizontal_{playerInput[i].horizontal}");


            EntityManager.AddComponentData<TC_MovingComponentData>(entities[i], new TC_MovingComponentData
            {
                Value = horizontal
            }); ;
            EntityManager.SetComponentData(entities[i], new DirectionData() { directionLook = horizontal > 0 ? 1 : -1});


        }

        entities.Dispose();
        playerInput.Dispose();
    }
}

public class S_PickupInput : ComponentSystem
{
    private EntityQuery m_EntityQuery;

    protected override void OnCreate()
    {
        m_EntityQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { ComponentType.ReadOnly<C_PlayerInput>(), ComponentType.ReadOnly<C_CanPick>() },
        });
    }

    protected override void OnUpdate()
    {
        var entities = m_EntityQuery.ToEntityArray(Allocator.TempJob);
        var playerInput = m_EntityQuery.ToComponentDataArray<C_PlayerInput>(Allocator.TempJob);

        for (int i = 0; i < playerInput.Length; i++)
        {
            if (Input.GetButton($"Action_{playerInput[i].action}"))
            {
                EntityManager.AddComponent<TC_PickHoldAction>(entities[i]);
            }
        }

        entities.Dispose();
        playerInput.Dispose();
    }
}