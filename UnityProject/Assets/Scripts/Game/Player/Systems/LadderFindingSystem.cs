using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class LadderFindingSystem : JobComponentSystem
{
    BuildPhysicsWorld m_BuildPhysicsWorldSystem;
    StepPhysicsWorld m_StepPhysicsWorldSystem;

    private EndSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    
    EntityQuery _entityQuery;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        m_BuildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
        
        _entityQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(C_LadderComponentData), }
        });
        
        m_EntityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        JobHandle handle = new LadderTrigger
        {
            ladders = GetComponentDataFromEntity<C_LadderComponentData>(),
            CommandBuffer = m_EntityCommandBuffer.CreateCommandBuffer(),
        }.Schedule(m_StepPhysicsWorldSystem.Simulation,
            ref m_BuildPhysicsWorldSystem.PhysicsWorld, inputDeps);
        
        
        m_EntityCommandBuffer.AddJobHandleForProducer(handle);
        return handle;
    }
    
    public struct LadderTrigger : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<C_LadderComponentData> ladders;
        public EntityCommandBuffer CommandBuffer;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity other = triggerEvent.Entities.EntityA;
            Entity ladder = triggerEvent.Entities.EntityB;

            if (ladders.HasComponent(ladder))
            {
                CommandBuffer.AddComponent<TC_IsInLadderArea>(other);
            }
        }
    }
}
