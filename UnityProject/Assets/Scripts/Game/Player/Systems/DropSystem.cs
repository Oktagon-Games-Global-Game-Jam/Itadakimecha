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
    
    private EntityQuery m_Query;
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        
    }
    
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        EntityCommandBuffer.Concurrent commandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();
        
        JobHandle jobHandle = Entities
            .WithAll<TC_PickHoldAction>()
            .WithNone<C_CanPick>()
            .ForEach((Entity entity, int entityInQueryIndex, in Translation translation, in C_HoldComponentData holdComponent, in DirectionData directionData) =>
            {
                int2 i2Direction = directionData.directionLook;
                float fXDropPosition = translation.Value.x + (float)i2Direction.x/2;
                
                commandBuffer.AddComponent<MC_RemoveInHold>(entityInQueryIndex, holdComponent.Item);
                commandBuffer.SetComponent(entityInQueryIndex, holdComponent.Item, new MC_RemoveInHold
                {
                    Position = new float3(fXDropPosition, translation.Value.y, 0)
                });
                commandBuffer.RemoveComponent<TC_PickHoldAction>(entityInQueryIndex, entity);
                commandBuffer.RemoveComponent<C_HoldComponentData>(entityInQueryIndex, entity);
                commandBuffer.AddComponent<C_CanPick>(entityInQueryIndex, entity);
                
                
            }).Schedule(inputDeps);
        
        
        jobHandle.Complete();

        JobHandle jobHandle2 = Entities
            .ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, in MC_RemoveInHold messageRemove, in TC_InHold inHold) =>
            {
                translation.Value = new float3(messageRemove.Position);
                commandBuffer.RemoveComponent<TC_InHold>(entityInQueryIndex, entity);
                commandBuffer.RemoveComponent<MC_RemoveInHold>(entityInQueryIndex, entity);
                commandBuffer.AddComponent<TC_Pickable>(entityInQueryIndex, entity);
            }).Schedule(jobHandle);
        
        jobHandle2.Complete();
        
        return jobHandle2;

    }

    
    
}
