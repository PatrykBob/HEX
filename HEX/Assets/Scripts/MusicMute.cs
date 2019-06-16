using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMute : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame

    void OnMouseDown()
    {
        if(AudioListener.volume != 0)
        {
            AudioListener.volume = 0;
        }
        else
            AudioListener.volume = 1;
    }

    void Update()
    {

    }
}
