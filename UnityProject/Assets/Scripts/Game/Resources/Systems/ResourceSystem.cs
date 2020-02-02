using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class ResourceSystem : JobComponentSystem
{
    private BeginSimulationEntityCommandBufferSystem _beginSimulationEntityCommandBuffer;

    protected override void OnCreate()
    {
        _beginSimulationEntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer.Concurrent commandBuffer = _beginSimulationEntityCommandBuffer.CreateCommandBuffer().ToConcurrent();
        JobHandle handle = Entities
            .ForEach((Entity entity, int entityInQueryIndex, ref C_ResourceComponentData data, in MC_ChangeResouceData changedData) =>
                {
                    data.CopperResources += changedData.DeltaCopper;
                    data.IronResources += changedData.DeltaIron;
                    
                    commandBuffer.RemoveComponent<MC_ChangeResouceData>(entityInQueryIndex, entity);
                })
            .Schedule(inputDeps);

        handle.Complete();

        return handle;

    }
}
