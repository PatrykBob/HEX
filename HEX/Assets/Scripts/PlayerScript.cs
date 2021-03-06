﻿using System;
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
    [SyncVar]
    public bool firstTurn = true;

    public List<string> Tokens = new List<string>();
    public List<string> TokensOnHand = new List<string>();

    public List<int> ToRemove = new List<int>();

    public GameObject token;
    public GameObject tokenUI;
    public Transform panel;
    public GameObject board;
    public GUIStyle customStyle;
    public GUIStyle customStyle2;


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
        board = GameObject.Find("Board");
    }

    [TargetRpc]
    public void TargetAssignFraction(NetworkConnection target, FractionEnum.Fraction fractionToAssign)
    {
        fraction = fractionToAssign;
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
        GetComponent<InputManagerScript>().SetFraction(fraction);
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

    [TargetRpc]
    public void TargetCheckBuffs(NetworkConnection target)
    {
        Debug.Log("Check player rpc");
        board.GetComponent<BoardScript>().CheckBuffs();
    }

    [Command]
    public void CmdCheckBuffs()
    {
        Debug.Log("Check player");
        ServManager.Instance.CheckBuffs();
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
    }

    [TargetRpc]
    public void TargetGiveTokens(NetworkConnection target)
    {
        if (!firstTurn)
        {
            FillTokens();
        }
        RemoveTokens();
        SpawnTokens();
    }

    void RemoveTokens()
    {
        foreach(Transform token in panel)
        {
            Destroy(token.gameObject);
        }
    }

    void SpawnTokens()
    {
        for (int i = 0; i < TokensOnHand.Count; i++)
        {
            GameObject toSpawn = Instantiate(tokenUI);
            toSpawn.transform.SetParent(panel);
            toSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300 + (300*i), 0);
            toSpawn.GetComponent<TokenUIScript>().name = TokensOnHand[i];
            toSpawn.GetComponent<TokenUIScript>().index = i;
            toSpawn.GetComponent<TokenUIScript>().LoadTexture();
        }
    }
    [Command]
    public void CmdBattle()
    {
        //board.GetComponent<BoardScript>().Battle();
        ServManager.Instance.Battle();
    }

    [TargetRpc]
    public void TargetBattle(NetworkConnection target)
    {
        board.GetComponent<BoardScript>().Battle();
    }

    [TargetRpc]
    public void TargetTurnOnHUD(NetworkConnection target)
    {
        panel.gameObject.SetActive(true);
    }

    public void TurnOnHUD()
    {
        panel.gameObject.SetActive(true);
    }

    public void TurnOffHUD()
    {
        panel.gameObject.SetActive(false);
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
        if (firstTurn)
        {
            firstTurn = false;
        }
    }

    void RemoveTokensFromList()
    {
        ToRemove.Sort();
        ToRemove.Reverse();
        foreach (var i in ToRemove)
        {
            TokensOnHand.RemoveAt(i);
        }
        ToRemove = new List<int>();
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
            GUI.Label(new Rect(100,300, buttonSizeW, buttonSizeH), ready ? "Gotowy" : "Nie gotowy",customStyle2);

            if (GUI.Button(new Rect(470, 100, 450, 300), Resources.Load<Texture>("Images/Buttons/ReadyButton"), customStyle))
            {
                CmdReady();
            }
        }
        else
        {
            if (myTurn && panel.gameObject.activeSelf)
            {
                if (GUI.Button(new Rect(500, 100, 450, 300), Resources.Load<Texture>("Images/Buttons/EndRoundButton"), customStyle))
                {
                    RemoveTokensFromList();
                    TurnOffHUD();
                    CmdAlterTurn();
                }
            }
        }
    }
}
