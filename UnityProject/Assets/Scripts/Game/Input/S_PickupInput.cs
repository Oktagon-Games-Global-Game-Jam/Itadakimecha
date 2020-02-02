using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class S_PickupInput : ComponentSystem
{
    private EntityQuery m_EntityQuery;

    protected override void OnCreate()
    {
        m_EntityQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { ComponentType.ReadOnly<PlayerInput_C>(), ComponentType.ReadOnly<TC_CanPick>() },
            None = new ComponentType[] { typeof(TC_CooldownAction) }
        });
    }

    protected override void OnUpdate()
    {
        var entities = m_EntityQuery.ToEntityArray(Allocator.TempJob);
        var playerInput = m_EntityQuery.ToComponentDataArray<PlayerInput_C>(Allocator.TempJob);

        for (int i = 0; i < playerInput.Length; i++)
        {
            if (Input.GetButton($"Action_{playerInput[i].inputId}"))
            {
                EntityManager.AddComponent<TC_PerformingAction>(entities[i]);
            }
        }

        entities.Dispose();
        playerInput.Dispose();
    }
}