using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class LadderExitAreaSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem m_EntityCommandBuffer;

    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var commandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();
        var entiyManager = EntityManager;

        //get all players inside ladders
        var jobHandle = Entities
            .ForEach((Entity entity, int entityInQueryIndex, in Translation translation, in C_IsInLadderArea ladder) =>
            {
                if (IsOutsideLadder(translation.Value, ladder))
                {
                    commandBuffer.RemoveComponent<C_IsInLadderArea>(entityInQueryIndex, entity);
                    if (ladder.freezeGravity)
                    {
                        commandBuffer.AddComponent<PhysicsVelocity>(entityInQueryIndex, entity);
                    }
                }

            }).Schedule(inputDeps);

        m_EntityCommandBuffer.AddJobHandleForProducer(inputDeps);

        return jobHandle;
    }

    private static bool IsOutsideLadder(in float3 position, in C_IsInLadderArea ladder)
    {
        return
            position.x < ladder.min.x || 
            position.x > ladder.max.x ||
            position.y < ladder.min.y || 
            position.y > ladder.max.y;
    }
}
