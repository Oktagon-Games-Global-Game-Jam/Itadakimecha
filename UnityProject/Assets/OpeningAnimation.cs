using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningAnimation : MonoBehaviour
{
    public GameObject mecha;
    public float value;

    public void SetMechaInPlace()
    {
        if (transform.localPosition.x > 30)
        {
            mecha.transform.Translate(Time.deltaTime * -value, 0, 0);
        }
    }
}
