using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

//[DisableAutoCreation]
public class TestAnimationSystem : ComponentSystem
{
    private EntityQuery m_EntityQuery;

    protected override void OnCreate()
    {
        m_EntityQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { ComponentType.ReadOnly<MonoAnimated_C>(), ComponentType.ReadOnly<C_MachineComponentData>() },
        });
    }

    protected override void OnUpdate()
    {
        var entities = m_EntityQuery.ToEntityArray(Allocator.TempJob);
        var caixas = m_EntityQuery.ToComponentDataArray<MonoAnimated_C>(Allocator.TempJob);

        for (int i = 0; i < caixas.Length; i++)
        {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.K))
            {
                EntityManager.AddComponentData(entities[i], new PlayMonoAnimation_C
                {
                    id = UnityEngine.Animator.StringToHash("Caixa@Closed")
                });
            }

            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.L))
            {
                EntityManager.AddComponentData(entities[i], new PlayMonoAnimation_C
                {
                    id = UnityEngine.Animator.StringToHash("Caixa@Open")
                });
            }
        }

        caixas.Dispose();
        entities.Dispose();
    }
}
