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

    public GameObject tokenUI;
    public GameObject canvas;

    public int buttonSizeW = 200;
    public int buttonSizeH = 200;
    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.FindChild("Canvas").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }
    [TargetRpc]
    public void TargetGiveTokens(NetworkConnection target)
    {
        GameObject toSpawn = Instantiate(tokenUI);
        toSpawn.transform.SetParent(canvas.transform);
        toSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -750);
        toSpawn.GetComponent<TokenUIScript>().name = "Red_Bloker";
        CmdGiveClientToken(toSpawn);
    }

    [Command]
    public void CmdGiveClientToken(GameObject toSpawn)
    {
        NetworkServer.SpawnWithClientAuthority(toSpawn, connectionToClient);
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

                if (GUI.Button(new Rect(10, 10, 200, 200), "Gotowość"))
                {
                    CmdReady();
                }
            }
            else
            {
                if (myTurn)
                {
                    if (GUI.Button(new Rect(10, 10, 200, 200), "Koniec tury"))
                    {
                        CmdAlterTurn();
                    }
                }
            }
        }
    }
}
