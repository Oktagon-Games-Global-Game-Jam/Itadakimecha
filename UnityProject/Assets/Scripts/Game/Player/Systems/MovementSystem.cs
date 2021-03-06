﻿using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Physics;

//[UpdateAfter(typeof(Unity.Physics.Systems.EndFramePhysicsSystem))]
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
            .ForEach((Entity entity, int entityInQueryIndex, in PhysicsVelocity velocity, in MovementComponentData moveData, in DirectionData directionData, in TC_MovingComponentData movingData) =>
            {
                //entityCommandBuffer.SetComponent(entityInQueryIndex, entity, new Translation { Value = new float3(trans.Value.x + (deltaTime * moveData.speed * movingData.Value), trans.Value.y, trans.Value.z) });
                entityCommandBuffer.SetComponent(entityInQueryIndex, entity, new PhysicsVelocity
                {
                    Angular = velocity.Angular,
                    Linear = new float3(moveData.speed * movingData.Value, velocity.Linear.y, velocity.Linear.z)
                });
                entityCommandBuffer.RemoveComponent<TC_MovingComponentData>(entityInQueryIndex, entity);
            })
            .Schedule(inputDeps);

        jobHandle.Complete();
        begin.AddJobHandleForProducer(inputDeps);

        return inputDeps;
    }
}
