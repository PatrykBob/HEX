using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameCode : MonoBehaviour
{
 
    // Start is called before the first frame update
    void Start()
    {
       
    }
    void OnMouseDown()
    {
        // Load the level named "HighScore".
        Application.LoadLevel("Main");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
