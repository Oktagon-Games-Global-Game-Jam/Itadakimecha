using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MachineConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public Animator m_Animator;

    [Header("Input")] 
    public ProductType InputResource;
    public int IronQnty;
    public int CopperQnty;

    [Header("Output")]
    public ProductType outputResource;

    [Header("Time to Complete")] 
    public int timeToComplete;
        
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData<C_MachineComponentData>(entity, new C_MachineComponentData
        {
            Input = InputResource,
            IronQnty = IronQnty,
            CopperQnty = CopperQnty,
            Result = outputResource,
            TimeToComplete = timeToComplete
        });

        dstManager.AddComponent<C_CooldownComponent>(entity);

        dstManager.AddComponentData(entity, new MonoAnimated_C
        {
            id = ECSMonoAnimation.Instance.AddAnimator(m_Animator)
        });
    }
}
