using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class GameInitializerSystem : JobComponentSystem
{
    
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;

    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        EntityCommandBuffer.Concurrent commandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();
        
        JobHandle fixRotation = Entities
            .WithAll<TC_InitializeFreezeAxes>()
            .ForEach((Entity entity, int entityInQueryIndex, ref PhysicsMass physicsMass) =>
            {
                physicsMass.InverseInertia = float3.zero;
                commandBuffer.RemoveComponent<TC_InitializeFreezeAxes>(entityInQueryIndex, entity);
            }).Schedule(inputDeps);
        
        fixRotation.Complete();

        return fixRotation;
    }
}
