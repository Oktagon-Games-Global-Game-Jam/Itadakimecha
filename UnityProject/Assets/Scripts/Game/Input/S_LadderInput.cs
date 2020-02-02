using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

//[UpdateAfter(typeof(ExportPhysicsWorld))]
public class S_LadderInput : ComponentSystem
{
    private EntityQuery m_EntityQuery;

    protected override void OnCreate()
    {
        m_EntityQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { ComponentType.ReadOnly<PlayerInput_C>(), ComponentType.ReadOnly<C_IsInLadderArea>() },
        });
    }

    protected override void OnUpdate()
    {
        var entities = m_EntityQuery.ToEntityArray(Allocator.TempJob);
        var playerInput = m_EntityQuery.ToComponentDataArray<PlayerInput_C>(Allocator.TempJob);
        var ladder = m_EntityQuery.ToComponentDataArray<C_IsInLadderArea>(Allocator.TempJob);

        for (int i = 0; i < playerInput.Length; i++)
        {
            float vertical = Input.GetAxis($"Vertical_{playerInput[i].inputId}");

            if (vertical == 0)
                continue;

            EntityManager.AddComponentData(entities[i], new C_LadderMovement
            {
                speed = vertical * 6
            });
            
            //freeze gravity after starting to climb the ladder
            if (!ladder[i].freezeGravity)
            {
                EntityManager.SetComponentData(entities[i], new C_IsInLadderArea
                {
                    freezeGravity = true,
                    min = ladder[i].min,
                    max = ladder[i].max
                });

                EntityManager.RemoveComponent<PhysicsVelocity>(entities[i]);
            }
        }

        entities.Dispose();
        playerInput.Dispose();
        ladder.Dispose();
    }
}