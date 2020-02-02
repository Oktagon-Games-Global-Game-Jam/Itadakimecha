using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaWalkOpening : MonoBehaviour
{

    void Update()
    {

        if (transform.localPosition.x > 30)
        {
            transform.Translate(Time.deltaTime * -5f, 0, 0);
        }
        else
        {
            Destroy(this);
        }
        
    }
}
