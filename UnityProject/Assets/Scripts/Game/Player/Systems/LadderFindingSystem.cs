using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class LadderFindingSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    
    EntityQuery m_LadderQuery;
    
    protected override void OnCreate()
    {        
        m_LadderQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(C_LadderComponentData) }
        });
        
        m_EntityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ladders = m_LadderQuery.ToComponentDataArray<C_LadderComponentData>(Allocator.TempJob);
        var commandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        //get all players
        var jobHandle = Entities.WithNone<TC_IsInLadderArea>().ForEach((Entity entity, int entityInQueryIndex, in Translation translation, in TC_CanClimbLadder climber) =>
        {
            //check each ladder
            for (int i = 0; i < ladders.Length; i++)
            {
                if (IsInsideLadder(translation.Value, ladders[i]))
                {
                    commandBuffer.AddComponent<TC_IsInLadderArea>(entityInQueryIndex, entity);
                    break;
                }
            }
        }).Schedule(inputDeps);

        jobHandle.Complete();

        ladders.Dispose();

        m_EntityCommandBuffer.AddJobHandleForProducer(inputDeps);

        return inputDeps;
    }

    private static bool IsInsideLadder(in float3 position, in C_LadderComponentData ladder)
    {
        return
            (position.x > ladder.min.x && position.x < ladder.max.x) &&
            (position.y > ladder.min.y && position.y < ladder.max.y);
    }
}
