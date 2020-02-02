using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kaiju : MonoBehaviour
{

    public int hp;
    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (true)//game is on
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);
        }
    }
}
