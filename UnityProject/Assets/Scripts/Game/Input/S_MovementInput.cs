﻿using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(CooldownSystem))]
public class S_MovementInput : ComponentSystem
{
    private EntityQuery m_EntityQuery;

    protected override void OnCreate()
    {
        m_EntityQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { ComponentType.ReadOnly<PlayerInput_C>(), ComponentType.ReadOnly<MovementComponentData>() },
        });
    }

    protected override void OnUpdate()
    {
        var entities = m_EntityQuery.ToEntityArray(Allocator.TempJob);
        var playerInput = m_EntityQuery.ToComponentDataArray<PlayerInput_C>(Allocator.TempJob);

        for (int i = 0; i < playerInput.Length; i++)
        {
            float horizontal = Input.GetAxis($"Horizontal_{playerInput[i].inputId}");
            
            EntityManager.AddComponentData<TC_MovingComponentData>(entities[i], new TC_MovingComponentData
            {
                Value = horizontal
            }); ;

            if (horizontal == 0)
            {
                EntityManager.AddComponentData(entities[i], new PlayMonoAnimation_C
                {
                    id = Animator.StringToHash("char_idle")
                });
            }
            else
            {

                int dir = horizontal > 0 ? 1 : -1;
                DirectionData data = EntityManager.GetComponentData<DirectionData>(entities[i]);
                data.directionLook.x = dir; //(int) math.ceil(horizontal) != 0 ? (int) math.ceil(horizontal) : data.directionLook.x;
                EntityManager.SetComponentData(entities[i], data);

                EntityManager.AddComponentData(entities[i], new PlayMonoAnimation_C
                {
                    id = Animator.StringToHash("char_run")
                });
            }

        }

        entities.Dispose();
        playerInput.Dispose();
    }
}