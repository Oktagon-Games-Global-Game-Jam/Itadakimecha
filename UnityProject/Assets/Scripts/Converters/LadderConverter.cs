using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class LadderConverter : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        BoxCollider collider = this.GetComponent<BoxCollider>();

        dstManager.AddComponentData(entity, new C_LadderComponentData
        {
            entity = entity,
            min = new float2(collider.bounds.min.x, collider.bounds.min.y),
            max = new float2(collider.bounds.max.x, collider.bounds.max.y)
        });

        Destroy(collider);
    }
}
