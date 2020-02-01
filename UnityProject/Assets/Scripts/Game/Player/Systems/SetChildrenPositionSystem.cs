using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(MovementSystem))]
public class SetChildrenPositionSystem : JobComponentSystem
{
    
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer.Concurrent commandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();
        
        JobHandle savePos = Entities.ForEach((Entity entity, int entityInQueryIndex, in Translation translation, in C_HoldComponentData holdData) =>
            {
                commandBuffer.AddComponent<C_SetPositionComponentData>(entityInQueryIndex, holdData.Item);
                commandBuffer.SetComponent(entityInQueryIndex, holdData.Item, new C_SetPositionComponentData
                {
                    Position = translation.Value,
                });
            }).Schedule(inputDeps);
        
        savePos.Complete();

        JobHandle updatePos = Entities.ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, in C_SetPositionComponentData setPositionComponentData) =>
        {
            if( translation.Value.Equals(setPositionComponentData.Position) ) {}
            else
            {
                translation.Value = new float3(setPositionComponentData.Position);
                commandBuffer.RemoveComponent<C_SetPositionComponentData>(entityInQueryIndex, entity);
            }

        }).Schedule(inputDeps);
        
        updatePos.Complete();

        return updatePos;
    }
}
