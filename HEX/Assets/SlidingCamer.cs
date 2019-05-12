﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingCamer : MonoBehaviour
{
    public float Speed;
    public bool shouldItMove = false;
    public bool MovingUp = false;
    // Start is called before the first frame update
    void Update()
    {

        if (shouldItMove == true)
        {
            transform.Translate(Vector3.down * Time.deltaTime * Speed);
            if (transform.position.y < -10)
            {

                shouldItMove = false;
            }
        }
        if(MovingUp == true)
        {
            transform.Translate(Vector3.down * Time.deltaTime * -Speed);
            if(transform.position.y > 1)
            {
                MovingUp = false;
            }
        }
    }
}