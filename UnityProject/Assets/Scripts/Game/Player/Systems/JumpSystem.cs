using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;
using Unity.Physics;

[AlwaysSynchronizeSystem]
public class JumpSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities.ForEach((ref PhysicsVelocity vel, in JumpComponentData jump) =>
        {
            if (Input.GetKeyDown(KeyCode.Space)) //input for jump
            {
                vel.Linear.y = jump.jumpForce;
            }

        }).Run();

        return inputDeps;
    }
}
