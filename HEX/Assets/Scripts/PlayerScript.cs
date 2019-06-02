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
    [SyncVar]
    public FractionEnum.Fraction fraction;

    public GameObject token;
    public GameObject tokenUI;
    public Transform panel;

    public int buttonSizeW = 200;
    public int buttonSizeH = 200;

    void Start()
    {
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false);
        }
        panel = transform.Find("Canvas").Find("Panel");
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Screen.height / 5);
    }

    [TargetRpc]
    public void TargetAssignFraction(NetworkConnection target, FractionEnum.Fraction fractionToAssign)
    {
        fraction = fractionToAssign;
    }

    [TargetRpc]
    public void TargetGiveTokens(NetworkConnection target)
    {
        Debug.Log("GiveToken" + tokenUI.name);
        GameObject toSpawn = Instantiate(tokenUI);
        toSpawn.transform.SetParent(panel);
        toSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        toSpawn.GetComponent<TokenUIScript>().name = "Red_Bloker";
    }

    [Command]
    public void CmdSpawnTokenOnServer(string name)
    {
        Debug.Log("SpawnServer: " + name + "conn " + connectionToClient);
        GameObject spawned = Instantiate(token, new Vector3(0, 1, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
        spawned.GetComponent<TokenScript>().name = name;
        NetworkServer.SpawnWithClientAuthority(spawned, connectionToClient);
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
            GUI.Label(new Rect(Screen.width / 2 - buttonSizeW / 2, Screen.height / 2 - buttonSizeH / 2, buttonSizeW, buttonSizeH), fraction.ToString());
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
