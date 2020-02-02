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
            x = new float2(collider.bounds.min.x, collider.bounds.max.x),
            y = new float2(collider.bounds.min.y, collider.bounds.max.y)
        });

        Destroy(collider);
    }
}
