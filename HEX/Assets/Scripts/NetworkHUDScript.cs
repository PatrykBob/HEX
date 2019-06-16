using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkHUDScript : MonoBehaviour
{
    // Start is called before the first frame update
    public NetworkManager manager;
    public bool showGUI = true;
    public GUIStyle customStyle;
    public GUIStyle customStyle2;
    public GUIStyle customStyle3;



    public bool GuiOnSolo = false;
    public bool GuiOnMulti = false;
    bool m_ShowServer;

    public void Start()
    {
        
    }

    private void Awake()
    {
        manager = GetComponent<NetworkManager>();
        manager.StartMatchMaker();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (!showGUI)
            return;

        int xpos = 10;
        int ypos = 40;
        const int spacing = 30;

        bool noConnection = (manager.client == null || manager.client.connection == null || manager.client.connection.connectionId == -1);

        if (NetworkServer.active || manager.IsClientConnected())
        {
            if (GUI.Button(new Rect(10, 100, 450, 300), Resources.Load<Texture>("Images/Buttons/Back"), customStyle))
            {
                GuiOnSolo = true;
            }
            ypos += 30;
        }

        if (GuiOnSolo)
        {
            GUI.Box(new Rect(Screen.width / 2 - 500, Screen.height / 2 - 300, 1000, 500), "Czy na pewno chcesz wyjsc?");
            if (GUI.Button(new Rect(Screen.width / 2 - 450, Screen.height / 2 - 200, 400, 300), "Tak"))
            {
                manager.StopHost();
                GuiOnSolo = false;
                //Application.Quit();
            }
            if (GUI.Button(new Rect(Screen.width / 2 + 50, Screen.height / 2 - 200, 400, 300), "Nie"))
            {
                GuiOnSolo = false;
            }
        }


        if (!NetworkServer.active && !manager.IsClientConnected() && noConnection)
        {
            ypos += 32;

            if (manager.matchMaker == null)
            {
                manager.StartMatchMaker();
            }
            else
            {
                if (manager.matchInfo == null)
                {
                    if (manager.matches == null)
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 450, 300), Resources.Load<Texture>("Images/Buttons/CreateMatchButton"), customStyle))
                        {
                            manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                        }
                        ypos += 300;

                        GUI.Label(new Rect(xpos, ypos, 100, 20), "Nazwa pokoju: ",customStyle2);
                        ypos += 80;
                        manager.matchName = GUI.TextField(new Rect(xpos , ypos, 450,150 ), manager.matchName,customStyle2);
                        ypos += 150;

                        

                        if (GUI.Button(new Rect(xpos, ypos, 450, 300), Resources.Load<Texture>("Images/Buttons/JoinGame"),customStyle)) 
                        {
                            manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);
                        }
                        ypos += 100;
                    }
                    else
                    {
                        for (int i = 0; i < manager.matches.Count; i++)
                        {
                            var match = manager.matches[i];
                            if (GUI.Button(new Rect(xpos, ypos, 450, 300), "Dołącz do: " + match.name))
                            {
                                manager.matchName = match.name;
                                manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                            }
                            ypos += 100;
                        }

                        if (GUI.Button(new Rect(xpos, ypos, 450, 300), Resources.Load<Texture>("Images/Buttons/Back"), customStyle))
                        {
                            GuiOnMulti = true;
                        }
                        ypos += 100;

                        if (GuiOnMulti)
                        {
                            GUI.Box(new Rect(Screen.width / 2 - 500, Screen.height / 2 - 300, 1000, 500), "Czy na pewno chcesz wyjsc?");
                            if (GUI.Button(new Rect(Screen.width / 2 - 450, Screen.height / 2 - 200, 400, 300), "Tak"))
                            {
                                manager.matches = null;
                                GuiOnMulti = false;
                                //Application.Quit();
                            }
                            if (GUI.Button(new Rect(Screen.width / 2 + 50, Screen.height / 2 - 200, 400, 300), "Nie"))
                            {
                                GuiOnMulti = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
