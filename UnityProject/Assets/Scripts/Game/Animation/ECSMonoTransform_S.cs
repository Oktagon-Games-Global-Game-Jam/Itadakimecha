using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class ECSMonoTransform_S : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem m_EntityCommandBuffer;

    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var commandBuffer = m_EntityCommandBuffer.CreateCommandBuffer();

        //var jobHandle = 
        Entities.ForEach((Entity entity, in MonoTransform_C transform, in Translation trans) =>
        {
            ECSMonoAnimation.Instance.SyncTransform(transform.id, trans.Value);
        })
        //.Schedule(inputDeps);
        .WithoutBurst()
        .Run();


        return inputDeps;
    }
}
