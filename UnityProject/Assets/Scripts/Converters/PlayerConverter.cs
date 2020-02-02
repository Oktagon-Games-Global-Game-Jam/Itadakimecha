using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class PlayerConverter : MonoBehaviour, IConvertGameObjectToEntity
{

    public int PickupDistance;
    public int JumpForce;
    public float Speed;
    public int PlayerIndex;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<DirectionData>(entity);

        dstManager.AddComponent<TC_InitializeFreezeAxes>(entity);
        
        dstManager.AddComponentData(entity, new PlayerInput_C
        {
            inputId = 0
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

        dstManager.AddComponent<TC_CanPick>(entity);
        dstManager.AddComponent<C_CooldownComponent>(entity);

    }
}
