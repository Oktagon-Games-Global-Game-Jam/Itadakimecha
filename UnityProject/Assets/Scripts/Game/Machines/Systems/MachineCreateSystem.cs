using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class MachineCreateSystem : JobComponentSystem
{
    private EntityQuery m_Query;
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        
        m_Query = GetEntityQuery(new EntityQueryDesc {
            All = new ComponentType[] { typeof(Prefab) },
        });
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        EntityCommandBuffer.Concurrent CommandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();        
        JobHandle handle = Entities
            .WithAll<TC_CooldownCompleted>()
            .WithNone<TC_CooldownRunning>()
            .ForEach((Entity entity, int entityInQueryIndex, in C_MachineComponentData data) =>
            {
                CommandBuffer.RemoveComponent<TC_CooldownCompleted>(entityInQueryIndex, entity);
                
                // SearchInPrefabs the prefab of data.Result

                Entity t = CommandBuffer.CreateEntity(entityInQueryIndex);
                CommandBuffer.AddComponent<C_ItemData>(entityInQueryIndex, t);
                CommandBuffer.SetComponent(entityInQueryIndex, t, new C_ItemData
                {
                    Type = data.Result
                });
                

            })
            .Schedule(inputDeps);
        
        handle.Complete();

        return handle;

    }
}
