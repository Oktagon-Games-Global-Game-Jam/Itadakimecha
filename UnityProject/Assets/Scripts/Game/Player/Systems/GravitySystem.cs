using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;

[AlwaysSynchronizeSystem]
public class GravitySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float gravity = 1; //use utils

        Entities.WithAll<JumpComponentData>().ForEach((ref PhysicsVelocity vel) =>
        {
            if (vel.Linear.y > 0)
            {
                vel.Linear.y -= deltaTime * gravity;
            }

        }).Run();

        return inputDeps;
    }
}
