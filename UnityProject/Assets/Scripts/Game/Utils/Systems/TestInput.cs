using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class TestInput : ComponentSystem
{
    protected override void OnUpdate()
    {
        bool dale = (Input.GetMouseButtonDown(0));

        Entities.WithAny<C_CanPick, C_HoldComponentData>().ForEach(
            (Entity entity) =>
            {
                if (dale)
                {
                    EntityManager.AddComponent<TC_PickHoldAction>(entity);
                }
            });
    }

}
