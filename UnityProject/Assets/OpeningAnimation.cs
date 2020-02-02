using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningAnimation : MonoBehaviour
{
    public GameObject mecha;
    public float value;

    public void SetMechaInPlace()
    {
        mecha.SetActive(true);
    }

}
