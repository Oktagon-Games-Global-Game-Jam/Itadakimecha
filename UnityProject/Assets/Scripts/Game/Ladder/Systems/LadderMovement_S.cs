using Unity.Physics;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(MovementSystem))]
public class LadderMovement_S : JobComponentSystem
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

        var jobHandle = Entities
            .ForEach((Entity entity, int entityInQueryIndex, in C_IsInLadderArea ladder, in C_LadderMovement movement, in Translation translation) =>
            {
                entityCommandBuffer.SetComponent(entityInQueryIndex, entity, new Translation
                {
                    Value = new float3(
                        translation.Value.x, 
                        translation.Value.y + (deltaTime * movement.speed),
                        translation.Value.z)
                });
                entityCommandBuffer.RemoveComponent<C_LadderMovement>(entityInQueryIndex, entity);
            })
            .Schedule(inputDeps);

        begin.AddJobHandleForProducer(inputDeps);

        return jobHandle;
    }
}
