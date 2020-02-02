using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
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
        
        JobHandle savePos = Entities.ForEach((Entity entity, int entityInQueryIndex, in Translation translation, in C_HoldComponentData holdData, in DirectionData directionData) =>
            {
                commandBuffer.SetComponent(entityInQueryIndex, holdData.Item, new C_SetPositionComponentData
                {
                    Position = translation.Value,
                });
            }).Schedule(inputDeps);
        
        savePos.Complete();

        JobHandle updatePos = Entities.ForEach((Entity entity, int entityInQueryIndex, ref Translation translation, ref PhysicsVelocity physicsVelocity, in C_SetPositionComponentData setPositionComponentData) =>
        {
            if( translation.Value.Equals(setPositionComponentData.Position) ) {}
            else
            {
                float3 finalPos = setPositionComponentData.Position;
                finalPos.y += 1.3f;
                physicsVelocity.Linear = float3.zero;
                physicsVelocity.Angular = float3.zero;
                translation.Value = finalPos;
            }

        }).Schedule(inputDeps);
        
        updatePos.Complete();

        return updatePos;
    }
}
