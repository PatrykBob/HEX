using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TokenUIScript : NetworkBehaviour
{
    public GameObject inputManager;

    private PlayerScript player;

    public string name;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RawImage>().texture = Resources.Load<Texture>("Images/Tokens/" + name);
        inputManager = GameObject.Find("InputManager");
        player = transform.parent.parent.parent.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Clicked()
    {
        player.CmdSpawnTokenOnServer(name);
    }
}
