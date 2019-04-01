using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
    [SyncVar]
    public bool myTurn = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnStartLocalPlayer()
    {

    }

    [Command]
    void CmdAlterTurn()
    {
        ServManager.Instance.AlterTurns();
    }

    void OnGUI()
    {
        if (isLocalPlayer)
        {
            GUI.Label(new Rect(10, 10, 100, 100), myTurn.ToString() + playerControllerId);
            if (GUI.Button(new Rect(10, 10, 100, 100), "Test"))
            {
                CmdAlterTurn();
            }
        }
    }
}
