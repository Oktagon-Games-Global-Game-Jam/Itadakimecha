using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Jobs;

public class ECSMonoAnimation_S : JobComponentSystem
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
            Entities.ForEach((Entity entity, in MonoAnimated_C animator, in PlayMonoAnimation_C animation) =>
            {
                ECSMonoAnimation.Instance.PlayAnimation(animator.id, animation.id);
                commandBuffer.RemoveComponent<PlayMonoAnimation_C>(entity);
                //EntityManager.RemoveComponent<PlayMonoAnimation_C>(entity);
            })
            //.Schedule(inputDeps);
            .WithoutBurst()
            .Run();

        m_EntityCommandBuffer.AddJobHandleForProducer(inputDeps);

        return inputDeps;
    }
}
