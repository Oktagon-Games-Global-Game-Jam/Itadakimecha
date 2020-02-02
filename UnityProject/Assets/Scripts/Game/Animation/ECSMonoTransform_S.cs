using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Jobs;

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
        Entities.ForEach((Entity entity, in MonoTransform_C transform, in SyncMonoTransform_C sync) =>
        {
            ECSMonoAnimation.Instance.SyncTransform(transform.id, sync.position);
            commandBuffer.RemoveComponent<PlayMonoAnimation_C>(entity);
        })
        //.Schedule(inputDeps);
        .WithoutBurst()
        .Run();


        return inputDeps;
    }
}
