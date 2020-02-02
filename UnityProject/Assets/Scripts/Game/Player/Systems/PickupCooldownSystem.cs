using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class PickupCooldownSystem : JobComponentSystem
{
    
    private EntityQuery m_Query;
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer.Concurrent CommandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();
        JobHandle jobHandle = Entities
            .WithAll<TC_CooldownCompleted, TC_CooldownAction, C_PickInfo>()
            .ForEach((Entity entity, int entityInQueryIndex) =>
            {
                CommandBuffer.RemoveComponent<TC_CooldownCompleted>(entityInQueryIndex, entity);
                CommandBuffer.RemoveComponent<TC_CooldownAction>(entityInQueryIndex, entity);
                CommandBuffer.AddComponent<TC_CanPick>(entityInQueryIndex, entity);
            }).Schedule(inputDeps);

        jobHandle.Complete();

        return jobHandle;

    }
}
