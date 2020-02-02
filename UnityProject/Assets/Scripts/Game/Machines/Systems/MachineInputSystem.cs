using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(PickupSystem))]
public class MachineInputSystem : JobComponentSystem
{
    
    private EntityQuery m_Query;
    private BeginSimulationEntityCommandBufferSystem m_EntityCommandBuffer;
    protected override void OnCreate()
    {
        m_EntityCommandBuffer = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        
        m_Query = GetEntityQuery(new EntityQueryDesc {
            All = new ComponentType[] { typeof(Translation), typeof(C_MachineComponentData) },
            None = new ComponentType[] { typeof(TC_CooldownRunning), typeof(TC_CreationCooldown)}
        });
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<Entity> lMachineEntities = m_Query.ToEntityArray(Allocator.TempJob);
        NativeArray<Translation> entitiesTranslation = m_Query.ToComponentDataArray<Translation>(Allocator.TempJob);
        EntityCommandBuffer.Concurrent commandBuffer = m_EntityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        // Find closest Machine and interact with
        JobHandle handle = Entities
            .WithAll<TC_PerformingAction>()
            .WithNone<C_HoldComponentData>()
            .ForEach((Entity entity, int entityInQueryIndex, in Translation translation, in C_PickInfo pickInfo, in DirectionData directionData) =>
            {
                int2 i2Direction = directionData.directionLook;
                float fMinLength = translation.Value.x;
                float fMaxLength = translation.Value.x + i2Direction.x * pickInfo.PickupDistance;
            
            
                int iClosestEntity = -1;
                float lastDistance = -1;
                for (int i = 0; i < entitiesTranslation.Length; i++)
                {

                    Translation objTranslation = new Translation {Value = entitiesTranslation[i].Value};

                    if (!Utils.IsInRange(objTranslation.Value.x, fMinLength, fMaxLength)) continue;
                    var actualDistance = Utils.CalculateDistance(objTranslation.Value.x, translation.Value.x);

                    if(lastDistance > actualDistance) continue;
                    lastDistance = actualDistance;
                    iClosestEntity = i;
                }
                if(iClosestEntity == -1) {}
                else
                {
                    commandBuffer.AddComponent<TC_CooldownAction>(entityInQueryIndex, entity);
                    commandBuffer.AddComponent<TC_CooldownRunning>(entityInQueryIndex, entity);
                    commandBuffer.SetComponent(entityInQueryIndex, entity, new C_CooldownComponent
                    {
                        Cooldown = 1,
                        DeltaTime = 0
                    });
                    
                    commandBuffer.AddComponent<TC_Interact>(entityInQueryIndex, lMachineEntities[iClosestEntity]);
                    commandBuffer.RemoveComponent<TC_PerformingAction>(entityInQueryIndex, entity);    
                }
                
            }).Schedule(inputDeps);

        lMachineEntities.Dispose(handle);
        entitiesTranslation.Dispose(handle);
        
        handle.Complete();

        return handle;

    }
}
