using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<C_CanPick>(entity);
        dstManager.AddComponent<Translation>(entity);
        dstManager.AddComponent<DirectionData>(entity);
        dstManager.AddComponent<MovementComponentData>(entity);
       
        dstManager.SetComponentData(entity, new C_CanPick
        {
            PickupDistance = 1
        });
       
        dstManager.SetComponentData(entity, new Translation
        {
            Value = transform.position,
        });
       
        dstManager.SetComponentData(entity, new DirectionData
        {
            //speed = 0,
            directionLook = new int2 { x = 1, y = 0 }
        });

        dstManager.SetComponentData(entity, new MovementComponentData
        {
            speed = 0,
        });
    }
}
