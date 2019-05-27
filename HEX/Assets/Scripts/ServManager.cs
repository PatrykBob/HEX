using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServManager : NetworkManager
{
    public int maxPlayers = 4;

    List<NetworkConnection> players;

    int activePlayer = 0;
    bool lobby = true;

    public static ServManager Instance;

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

    void Update()
    {
        if (lobby)
        {
            if (players.Count > 1)
            {
                bool ready = true;
                foreach (var player in players)
                {
                    if (!player.playerControllers[0].gameObject.GetComponent<PlayerScript>().ready)
                    {
                        ready = false;
                        break;
                    }
                }
                if (ready && players.Count > 1)
                {
                    StartGame();
                }
            }
        }
    }

    public void StartGame()
    {
        lobby = false;
        foreach (var player in players)
        {
            player.playerControllers[0].gameObject.GetComponent<PlayerScript>().lobby = false;
        }
        players[0].playerControllers[0].gameObject.GetComponent<PlayerScript>().myTurn = true;
        Debug.Log("game started");
    }

    public void AlterTurns()
    {
        Debug.Log("Zmiana gracza");
        foreach (var player in players)
        {
            player.playerControllers[0].gameObject.GetComponent<PlayerScript>().myTurn = false;
        }
        activePlayer++;
        activePlayer %= players.Count;
        players[activePlayer].playerControllers[0].gameObject.GetComponent<PlayerScript>().myTurn = true;
        players[activePlayer].playerControllers[0].gameObject.GetComponent<PlayerScript>().TargetGiveTokens(players[activePlayer]);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (lobby && players.Count < maxPlayers)
        {
            Debug.Log(conn);
            players.Add(conn);
        }
        else
        {
            conn.Disconnect();
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (lobby)
        {
            players.Remove(conn);
        }
    }
}
