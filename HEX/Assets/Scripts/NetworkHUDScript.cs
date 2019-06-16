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
        const int spacing = 24;

        bool noConnection = (manager.client == null || manager.client.connection == null || manager.client.connection.connectionId == -1);

        if (NetworkServer.active || manager.IsClientConnected())
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 200), Resources.Load<Texture>("Images/MenuButtons/QuitButton"), customStyle))
            {
                manager.StopHost();
            }
            ypos += spacing;
        }

        if (!NetworkServer.active && !manager.IsClientConnected() && noConnection)
        {
            ypos += 10;

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
                        if (GUI.Button(new Rect(xpos, ypos, 200, 200), Resources.Load<Texture>("Images/Buttons/CreateMatchButton"), customStyle))
                        {
                            manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                        }
                        ypos += spacing;

                        GUI.Label(new Rect(xpos, ypos, 100, 20), "Nazwa pokoju: ");
                        manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), manager.matchName);
                        ypos += spacing;

                        ypos += 10;

                        if (GUI.Button(new Rect(xpos, ypos, 200, 200), Resources.Load<Texture>("Images/MenuButtons/NewGameButton"), customStyle)) 
                        {
                            manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);
                        }
                        ypos += spacing;
                    }
                    else
                    {
                        for (int i = 0; i < manager.matches.Count; i++)
                        {
                            var match = manager.matches[i];
                            if (GUI.Button(new Rect(xpos, ypos, 200, 200), "Dołącz do: " + match.name))
                            {
                                manager.matchName = match.name;
                                manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                            }
                            ypos += spacing;
                        }

                        if (GUI.Button(new Rect(xpos, ypos, 200, 200), Resources.Load<Texture>("Images/MenuButtons/BackButton"), customStyle))
                        {
                            manager.matches = null;
                        }
                        ypos += spacing;
                    }
                }
            }
        }
    }
}
