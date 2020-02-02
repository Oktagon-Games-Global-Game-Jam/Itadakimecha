using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Physics.Systems;

//[UpdateInGroup(typeof(PresentationSystemGroup))]
public class ECSMonoTransform_S : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //var jobHandle = 
        Entities.ForEach((Entity entity, in MonoTransform_C transform, in Translation trans) =>
        {
            ECSMonoAnimation.Instance.SyncTransform(transform.id, trans.Value);
        })
        //.Schedule(inputDeps);
        .WithoutBurst()
        .Run();


        return inputDeps;
    }
}
