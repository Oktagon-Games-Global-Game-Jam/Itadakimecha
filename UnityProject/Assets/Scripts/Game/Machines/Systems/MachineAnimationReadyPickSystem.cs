using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class MachineAnimationReadyPickSystem : JobComponentSystem
{
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer.Concurrent CommandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        JobHandle handle = Entities
            .WithNone<TC_CooldownRunning>()
            .WithAll<TC_CooldownCompleted, C_MachineComponentData>()
            .ForEach(
                (Entity entity, int entityInQueryIndex) =>
                {
                    CommandBuffer.AddComponent<PlayMonoAnimation_C>(entityInQueryIndex, entity);
                    CommandBuffer.SetComponent(entityInQueryIndex, entity, new PlayMonoAnimation_C
                    {
                        id = UnityEngine.Animator.StringToHash("Caixa@Open")
                    });

                })
            .WithoutBurst()
            .Schedule(inputDeps);
        
        handle.Complete();

        return handle;
    }
}
