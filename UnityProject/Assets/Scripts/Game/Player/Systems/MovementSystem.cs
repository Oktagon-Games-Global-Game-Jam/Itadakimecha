using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Physics;

//[AlwaysSynchronizeSystem]
public class MovementSystem : JobComponentSystem
{
    public BeginSimulationEntityCommandBufferSystem begin;

    protected override void OnCreate()
    {
        begin = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var entityCommandBuffer = begin.CreateCommandBuffer().ToConcurrent();

        float deltaTime = Time.DeltaTime;

        JobHandle jobHandle = 
            Entities
            .ForEach((Entity entity, int entityInQueryIndex, in Translation trans, in MovementComponentData moveData, in DirectionData directionData, in TC_MovingComponentData movingData) =>
            {
                entityCommandBuffer.SetComponent(entityInQueryIndex, entity, new Translation { Value = new float3(trans.Value.x + (deltaTime * moveData.speed * movingData.Value), trans.Value.y, trans.Value.z) });
                entityCommandBuffer.RemoveComponent<TC_MovingComponentData>(entityInQueryIndex, entity);
            })
            .Schedule(inputDeps);

        //jobHandle.Complete();
        begin.AddJobHandleForProducer(inputDeps);

        return jobHandle;
    }
}
