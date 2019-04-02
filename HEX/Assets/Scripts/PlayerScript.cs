using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
    [SyncVar]
    public bool myTurn = false;
    [SyncVar]
    public bool lobby = true;
    [SyncVar]
    public bool ready = false;

    public int buttonSizeW = 200;
    public int buttonSizeH = 200;
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

    [Command]
    void CmdReady()
    {
        ready = !ready;
    }

    void OnGUI()
    {
        if (isLocalPlayer)
        {
            if (lobby)
            {
                GUI.Label(new Rect(Screen.width / 2 - buttonSizeW / 2, Screen.height / 2 - buttonSizeH / 2, buttonSizeW, buttonSizeH), ready ? "Gotowy" : "Nie gotowy");

                if (GUI.Button(new Rect(10, 10, 100, 100), "Gotowość"))
                {
                    CmdReady();
                }
            }
            else
            {
                if (GUI.Button(new Rect(10, 10, 100, 100), "Koniec tury"))
                {
                    CmdAlterTurn();
                }
            }
        }
    }
}
