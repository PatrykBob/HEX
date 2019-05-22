using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenUIScript : MonoBehaviour
{
    public GameObject token;
    public GameObject inputManager;

    public string name;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RawImage>().texture = Resources.Load<Texture>("Images/Tokens/" + name);
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Clicked()
    {
        GameObject spawned = Instantiate(token, new Vector3(0,1,0), Quaternion.Euler(new Vector3(-90,0,0)));
        spawned.GetComponent<TokenScript>().tokenObject = Resources.Load<TokenScriptableObject>("Tokens/" + name);
        inputManager.GetComponent<InputManagerScript>().selectedToken = spawned;
        inputManager.GetComponent<InputManagerScript>().inPlace = false;
    }
}
