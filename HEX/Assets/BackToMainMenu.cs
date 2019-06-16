using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMainMenu : MonoBehaviour
{
    public SlidingCamer Booly;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject g = Camera.main.gameObject;
        //Booly = g.GetComponent<SlidingCamer>();

    }
    void OnMouseDown()
    {
        Booly.MovingUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
