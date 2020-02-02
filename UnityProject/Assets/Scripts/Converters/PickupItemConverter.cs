using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PickupItemConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public ProductType ItemType;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<TC_Pickable>(entity);
        dstManager.AddComponent<Prefab>(entity);
        dstManager.AddComponentData<C_ItemData>(entity, new C_ItemData
        {
            Type = ItemType,
        });
    }
}
