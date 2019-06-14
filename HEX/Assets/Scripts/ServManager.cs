using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServManager : NetworkManager
{
    public int maxPlayers = 4;

    List<NetworkConnection> players;

    List<FractionEnum.Fraction> fractions = new List<FractionEnum.Fraction> {
        FractionEnum.Fraction.moloch,
        FractionEnum.Fraction.posterunek,
        FractionEnum.Fraction.borgo,
        FractionEnum.Fraction.hegemonia };


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

    public void CheckBuffs()
    {
        foreach(var player in players)
        {
            player.playerControllers[0].gameObject.GetComponent<PlayerScript>().TargetCheckBuffs(player);
        }
    }

    public void StartGame()
    {
        lobby = false;

        AssignFractionsToPlayers();

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

    public void RandomizeFractions()
    {
        for (int i = 0; i < fractions.Count; i++)
        {
            int random = Random.Range(0, fractions.Count);
            FractionEnum.Fraction temp;
            temp = fractions[i];
            fractions[i] = fractions[random];
            fractions[random] = temp;
        }
    }

    public void Battle()
    {
        foreach(var player in players)
        {
            player.playerControllers[0].gameObject.GetComponent<PlayerScript>().TargetBattle(player);
        }
    }

    public void AssignFractionsToPlayers()
    {
        RandomizeFractions();
        for (int i = 0; i < players.Count; i++)
        {
            players[i].playerControllers[0].gameObject.GetComponent<PlayerScript>().TargetAssignFraction(players[i], fractions[i]);
        }
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
