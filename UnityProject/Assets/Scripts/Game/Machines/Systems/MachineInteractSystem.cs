using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class MachineInteractSystem : JobComponentSystem
{
    private EntityQuery m_Query;
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        
        m_Query = GetEntityQuery(new EntityQueryDesc {
            All = new ComponentType[] { typeof(C_ResourceComponentData) },
        });
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<Entity> resourceEntities = m_Query.ToEntityArray(Allocator.TempJob);
        NativeArray<C_ResourceComponentData> resources = m_Query.ToComponentDataArray<C_ResourceComponentData>(Allocator.TempJob);
        EntityCommandBuffer.Concurrent commandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();
        
        JobHandle handle = Entities
            .WithAll<TC_Interact>()
            .WithNone<TC_CooldownRunning, TC_CooldownCompleted, TC_CreationCooldown>()
            .ForEach((Entity entity, int entityInQueryIndex, ref C_MachineComponentData data) =>
            {
                if (resources.Length == 0) {}
                else
                {
                    if (resources[0].CopperResources >= data.CopperQnty && resources[0].IronResources >= data.IronQnty)
                    {
                        commandBuffer.AddComponent<MC_ChangeResouceData>(entityInQueryIndex, resourceEntities[0]);
                        commandBuffer.SetComponent(entityInQueryIndex, resourceEntities[0], new MC_ChangeResouceData
                        {
                            DeltaCopper = -data.CopperQnty,
                            DeltaIron = -data.IronQnty,
                        });
                        
                        commandBuffer.SetComponent(entityInQueryIndex, entity, new C_CooldownComponent()
                        {
                            DeltaTime = 0,
                            Cooldown = data.TimeToComplete,
                        });
                        commandBuffer.AddComponent<TC_CooldownRunning>(entityInQueryIndex, entity);
                    }
                }
                
                commandBuffer.RemoveComponent<TC_Interact>(entityInQueryIndex, entity);
                
            }).Schedule(inputDeps);
        
        handle.Complete();

        resourceEntities.Dispose(handle);
        resources.Dispose(handle);

        return handle;
    }
}
