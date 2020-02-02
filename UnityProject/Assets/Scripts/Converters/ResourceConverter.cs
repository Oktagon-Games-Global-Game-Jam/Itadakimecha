using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(ConvertToEntity))]
public class ResourceConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new C_ResourceComponentData
        {
            CopperResources = 20,
            IronResources = 20
        });
    }
}
