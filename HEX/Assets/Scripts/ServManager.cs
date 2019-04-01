using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServManager : NetworkManager
{

    List<NetworkConnection> players;
    int activePlayer = 0;

    public static ServManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        players = new List<NetworkConnection>();
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AlterTurns()
    {
        Debug.Log("Zmiana gracza");
        foreach(var player in players)
        {
            player.playerControllers[0].gameObject.GetComponent<PlayerScript>().myTurn = false;
        }
        activePlayer++;
        activePlayer %= players.Count;
        players[activePlayer].playerControllers[0].gameObject.GetComponent<PlayerScript>().myTurn = true;
    }


    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log(conn);
        players.Add(conn);
        Debug.Log(conn.playerControllers.Count);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        Debug.Log("added");
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        players.Remove(conn);
    }

}
