using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kaiju : MonoBehaviour
{

    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed, 0, 0);
    }
}
