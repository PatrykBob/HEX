﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraMove : MonoBehaviour
{


    public SlidingCamer Booly;
   
    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("Cam");
        Booly = g.GetComponent<SlidingCamer>();
        
    }
    void OnMouseDown()
    {
        Booly.shouldItMove = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
