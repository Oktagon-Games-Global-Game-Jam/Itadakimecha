using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(PickupSystem))]
public class DropSystem : JobComponentSystem
{
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        
    }
    
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        EntityCommandBuffer.Concurrent commandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();
        
        
        JobHandle jobConfigureDrop = Entities
            .WithAll<TC_PerformingAction>()
            .ForEach((Entity entity, int entityInQueryIndex, in Translation translation, in C_HoldComponentData holdComponent, in DirectionData directionData) =>
            {
                int2 i2Direction = directionData.directionLook;
                float fXDropPosition = translation.Value.x + (float)i2Direction.x/2;
                
                commandBuffer.AddComponent<MC_RemoveInHold>(entityInQueryIndex, holdComponent.Item);
                commandBuffer.SetComponent(entityInQueryIndex, holdComponent.Item, new MC_RemoveInHold
                {
                    Position = new float3(fXDropPosition, translation.Value.y, 0)
                });
                commandBuffer.RemoveComponent<TC_PerformingAction>(entityInQueryIndex, entity);
                commandBuffer.RemoveComponent<C_HoldComponentData>(entityInQueryIndex, entity);
                commandBuffer.AddComponent<TC_CooldownAction>(entityInQueryIndex, entity);
                commandBuffer.AddComponent<TC_CooldownRunning>(entityInQueryIndex, entity);
                commandBuffer.SetComponent(entityInQueryIndex, entity, new C_CooldownComponent
                {
                    Cooldown = 1,
                    DeltaTime = 0
                });
                
                
            }).Schedule(inputDeps);
        
        
        jobConfigureDrop.Complete();

        JobHandle jobHandle2 = Entities
            .ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, in MC_RemoveInHold messageRemove, in TC_InHold inHold) =>
            {
                translation.Value = new float3(messageRemove.Position);
                commandBuffer.RemoveComponent<TC_InHold>(entityInQueryIndex, entity);
                commandBuffer.RemoveComponent<MC_RemoveInHold>(entityInQueryIndex, entity);
                commandBuffer.AddComponent<TC_Pickable>(entityInQueryIndex, entity);
                commandBuffer.RemoveComponent<C_SetPositionComponentData>(entityInQueryIndex, entity);
            }).Schedule(jobConfigureDrop);
        
        jobHandle2.Complete();
        
        return jobHandle2;

    }

    
    
}
