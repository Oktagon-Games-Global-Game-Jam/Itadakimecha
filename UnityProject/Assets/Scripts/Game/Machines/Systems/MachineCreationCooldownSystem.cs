using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class MachineCreationCooldownSystem : JobComponentSystem
{
    
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer.Concurrent CommandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        JobHandle handle = Entities
            .WithNone<TC_CooldownRunning>()
            .WithAll<TC_CooldownCompleted, TC_CreationCooldown>()
            .ForEach(
                (Entity entity, int entityInQueryIndex) =>
                {
                    CommandBuffer.RemoveComponent<TC_CreationCooldown>(entityInQueryIndex, entity);
                    CommandBuffer.RemoveComponent<TC_CooldownCompleted>(entityInQueryIndex, entity);
                    
                }).Schedule(inputDeps);
        
        handle.Complete();

        return handle;
    }
}
