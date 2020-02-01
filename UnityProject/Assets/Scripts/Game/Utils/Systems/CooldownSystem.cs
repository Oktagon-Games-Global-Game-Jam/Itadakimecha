using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class CooldownSystem : JobComponentSystem
{
    private EntityQuery m_Query;
    private EndSimulationEntityCommandBufferSystem m_EndSimulationEntityCommandBufferSystem;
    
    
    protected override void OnCreate()
    {
        base.OnCreate();
        m_Query = GetEntityQuery(new ComponentType[]
        {
            ComponentType.Exclude<TC_CooldownCompleted>(),
            ComponentType.ReadWrite<C_CooldownComponent>(), 
        });

        m_EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {

        NativeArray<Entity> lEntities = m_Query.ToEntityArray(Allocator.TempJob); 
        
        JobHandle handle = new J_DecreaseCooldownSystem
        {
            ActualDeltaTime = Time.DeltaTime,
            ChunkCooldownComponent = GetArchetypeChunkComponentType<C_CooldownComponent>(),
            CommandBuffer = m_EndSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
            Entities = lEntities
        }.Schedule(m_Query, inputDeps);

        handle.Complete();
        lEntities.Dispose(inputDeps);

        return handle;

    }
    

    public struct J_DecreaseCooldownSystem : IJobChunk
    {
        public ArchetypeChunkComponentType<C_CooldownComponent> ChunkCooldownComponent;
        public NativeArray<Entity> Entities;
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public float ActualDeltaTime;
        
        
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<C_CooldownComponent> cooldownComponents = chunk.GetNativeArray(ChunkCooldownComponent);
            
            for (int i = 0; i < chunk.Count; i++)
            {

                if (cooldownComponents[i].DeltaTime >= cooldownComponents[i].Cooldown)
                {
                    CommandBuffer.AddComponent<TC_CooldownCompleted>(chunkIndex, Entities[chunkIndex + i]);
                }
                else
                {
                    cooldownComponents[i] = new C_CooldownComponent
                    {
                        Cooldown = cooldownComponents[i].Cooldown,
                        DeltaTime = cooldownComponents[i].DeltaTime + ActualDeltaTime
                    };
                }
            }
        }
    }
    
}
