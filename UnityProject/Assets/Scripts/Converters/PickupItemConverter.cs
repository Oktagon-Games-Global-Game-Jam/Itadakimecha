using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PickupItemConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<TC_Pickable>(entity);
        dstManager.AddComponent<Translation>(entity);
        dstManager.SetComponentData(entity, new Translation
        {
            Value = transform.position
        });
        
    }
}
