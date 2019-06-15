using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TokenUIScript : NetworkBehaviour
{
    public GameObject inputManager;

    private PlayerScript player;

    public int index;

    public string name;
    // Start is called before the first frame update
    void Start()
    {        
        player = transform.parent.parent.parent.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadTexture()
    {
        GetComponent<RawImage>().texture = Resources.Load<Texture>("Images/Tokens/" + name);
    }
    public void Clicked()
    {
        if (name == "Red_Bitwa" || name == "Yellow_Bitwa" || name == "Blue_Bitwa" || name == "Green_Bitwa")
        {
            player.CmdBattle();
        }
        else
        {
            player.CmdSpawnTokenOnServer(name);
            player.TurnOffHUD();
        }
            player.RemoveToken(index);
            Destroy(this.gameObject);
    }
}
