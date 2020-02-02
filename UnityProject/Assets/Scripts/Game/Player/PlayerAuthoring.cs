using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField]
    private int m_InputId;

    [SerializeField]
    private Animator m_Animator;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent(entity, typeof(TC_CanPick));
        dstManager.AddComponent(entity, typeof(DirectionData));

        dstManager.AddComponentData(entity, new PlayerInput_C
        {
            inputId = 0
        });
        dstManager.AddComponentData(entity, new MonoAnimated_C
        {
            id = ECSMonoAnimation.Instance.AddAnimator(m_Animator)
        });
        dstManager.AddComponentData(entity, new MonoTransform_C
        {
            id = ECSMonoAnimation.Instance.AddTransform(this.transform)
        });
        dstManager.AddComponentData(entity, new MovementComponentData
        {
            speed = 8
        });
    }
}
