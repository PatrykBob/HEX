﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenUIScript : MonoBehaviour
{
    public GameObject token;
    public GameObject inputManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Clicked()
    {
        GameObject spawned = Instantiate(token, new Vector3(0,1,0), Quaternion.Euler(new Vector3(-90,0,0)));
        inputManager.GetComponent<InputManagerScript>().SelectedToken = spawned;
        transform.parent.gameObject.SetActive(false);
    }
}
