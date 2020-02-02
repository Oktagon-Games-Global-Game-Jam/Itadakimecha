using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class PlayerConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public Animator m_Animator;
    public int PickupDistance;
    public int JumpForce;
    public float Speed;
    public int PlayerIndex;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        //setup entity
        dstManager.AddComponent<DirectionData>(entity);
        dstManager.AddComponent<TC_InitializeFreezeAxes>(entity);
        dstManager.AddComponent<TC_CanPick>(entity);
        dstManager.AddComponent<C_CooldownComponent>(entity);
        dstManager.AddComponent<TC_CanClimbLadder>(entity);

        dstManager.AddComponentData(entity, new PlayerInput_C
        {
            inputId = PlayerIndex
        });
        
        dstManager.AddComponentData(entity, new JumpComponentData
        {
            jumpForce = JumpForce
        });
        
        dstManager.AddComponentData(entity, new MovementComponentData
        {
            speed = Speed
        });

        dstManager.AddComponentData(entity, new C_PickInfo
        {
            PickupDistance = PickupDistance
        });

        dstManager.AddComponentData(entity, new MonoAnimated_C
        {
            id = ECSMonoAnimation.Instance.AddAnimator(m_Animator)
        });
        dstManager.AddComponentData(entity, new MonoTransform_C
        {
            id = ECSMonoAnimation.Instance.AddTransform(this.transform)
        });

        //remove unused monobehaviors
        Destroy(this.GetComponent<UnityEngine.BoxCollider>());
        Destroy(this.GetComponent<UnityEngine.Rigidbody>());
    }
}
