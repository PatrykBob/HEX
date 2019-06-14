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
<<<<<<< Updated upstream
=======
        GetTokenList();
        ShuffleTokens();
        string sztab = "";
        if(fraction == FractionEnum.Fraction.borgo)
        {
            sztab = "Blue_Sztab";
        }
        else if(fraction == FractionEnum.Fraction.posterunek)
        {
            sztab = "Green_Sztab";
        }
        else if (fraction == FractionEnum.Fraction.hegemonia)
        {
            sztab = "Yellow_Sztab";
        }
        else if (fraction == FractionEnum.Fraction.moloch)
        {
            sztab = "Red_Sztab";
        }
        TokensOnHand.Add(sztab);
    }

    void GetTokenList()
    {
        TextAsset list = (TextAsset)Resources.Load("TokenLists/" + fraction.ToString(), typeof(TextAsset));
        string[] lines = list.text.Split("\n\r".ToCharArray());
        foreach (var line in lines)
        {
            if (line != "")
            {
                Tokens.Add(line);
            }
        }
    }

    [ClientRpc]
    public void RpcCheckBuffs()
    {
        Debug.Log("Check player rpc");
        board.GetComponent<BoardScript>().CheckBuffs();
    }

    public void CheckBuffs()
    {
        Debug.Log("Check player");
        ServManager.Instance.CheckBuffs();
        //RpcCheckBuffs();
    }

    void FillTokens()
    {
        int count = TokensOnHand.Count;
        if (count < 3)
        {
            for (int i = 0; i < 3 - count; i++)
            {
                TokensOnHand.Add(Tokens[0]);
                Tokens.RemoveAt(0);
            }
        }
    }
    
    public void RemoveToken(int i)
    {
        ToRemove.Add(i);
    }

    void ShuffleTokens()
    {
        for (int i = 0; i < Tokens.Count; i++)
        {
            int random = UnityEngine.Random.Range(0, Tokens.Count);
            string temp;
            temp = Tokens[i];
            Tokens[i] = Tokens[random];
            Tokens[random] = temp;
        }
>>>>>>> Stashed changes
    }

    [TargetRpc]
    public void TargetGiveTokens(NetworkConnection target)
    {
        Debug.Log("GiveToken" + tokenUI.name);
        GameObject toSpawn = Instantiate(tokenUI);
        toSpawn.transform.SetParent(panel);
        toSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        toSpawn.GetComponent<TokenUIScript>().name = "Red_Zwiadowca";

        GameObject toSpawn2 = Instantiate(tokenUI);
        toSpawn2.transform.SetParent(panel);
        toSpawn2.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 0);
        toSpawn2.GetComponent<TokenUIScript>().name = "Red_Hybryda";
    }

    [Command]
    public void CmdSpawnTokenOnServer(string name)
    {
        Debug.Log("SpawnServer: " + name + "conn " + connectionToClient);
        GameObject spawned = Instantiate(token, new Vector3(0, 1, 0), Quaternion.Euler(new Vector3(-90, 0, 0)));
        spawned.name = name;
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
                    RemoveTokensFromList();
                    CmdAlterTurn();
                }
            }
        }
    }

}
