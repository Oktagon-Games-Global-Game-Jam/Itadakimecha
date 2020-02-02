using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MachineCreateSystem : JobComponentSystem
{
    private EntityQuery m_Query;
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        
        m_Query = GetEntityQuery(new EntityQueryDesc {
            All = new ComponentType[] { ComponentType.ReadOnly<Prefab>(), ComponentType.ReadOnly<C_ItemData>(),   },
        });
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        NativeArray<Entity> lPartsPrefabs = m_Query.ToEntityArray(Allocator.TempJob);
        NativeArray<C_ItemData> lPartsPrefabsData = m_Query.ToComponentDataArray<C_ItemData>(Allocator.TempJob);
       
        EntityCommandBuffer.Concurrent CommandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();        
        JobHandle handle = Entities
            .WithAll<TC_CooldownCompleted, TC_Interact>()
            .WithNone<TC_CooldownRunning>()
            .ForEach((Entity entity, int entityInQueryIndex, in C_MachineComponentData data, in Translation translation) =>
            {
                CommandBuffer.RemoveComponent<TC_CooldownCompleted>(entityInQueryIndex, entity);

                for (int i = 0; i < lPartsPrefabs.Length; i++)
                {
                    if(lPartsPrefabsData[i].Type != data.Result) continue;
                    Entity t = CommandBuffer.Instantiate(entityInQueryIndex, lPartsPrefabs[i]);
                    CommandBuffer.SetComponent(entityInQueryIndex, t, new Translation
                    {
                        Value = new float3
                        {
                            x = translation.Value.x,
                            y = translation.Value.y,
                            z = 0, 
                        }
                    });
                }

                CommandBuffer.SetComponent(entityInQueryIndex, entity, new C_CooldownComponent()
                {
                    DeltaTime = 0,
                    Cooldown = 2,
                });
                CommandBuffer.AddComponent<TC_CreationCooldown>(entityInQueryIndex, entity);
                CommandBuffer.AddComponent<TC_CooldownRunning>(entityInQueryIndex, entity);
                CommandBuffer.AddComponent<PlayMonoAnimation_C>(entityInQueryIndex, entity);
                CommandBuffer.SetComponent(entityInQueryIndex, entity, new PlayMonoAnimation_C
                {
                    id = UnityEngine.Animator.StringToHash("Caixa@Closed")
                });
                CommandBuffer.RemoveComponent<TC_Interact>(entityInQueryIndex, entity);
                

            })
            .WithoutBurst()
            .Schedule(inputDeps);
        
        handle.Complete();

        lPartsPrefabs.Dispose(handle);
        lPartsPrefabsData.Dispose(handle);

        return handle;

    }
}
