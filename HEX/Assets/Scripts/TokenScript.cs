using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TokenScript : NetworkBehaviour
{
    public int i;
    public int j;

    [SyncVar]
    public string name;
    [SyncVar]
    public int health = 1;
    [SyncVar]
    public bool netted = false;
    [SyncVar]
    public bool doubleAttack = false;
    [SyncVar]
    public int rangeBuff = 0;
    [SyncVar]
    public int meleeBuff = 0;
    [SyncVar]
    public int initiation = 0;
    [SyncVar]
    public bool wasMoved = false;
    [SyncVar]
    public bool mobility;
    [SyncVar]
    public int rotation = 0;

    public TokenScriptableObject tokenObject;

    void Start()
    {
        if (!tokenObject)
        {
            Debug.Log("Brak tokenObject " + name);
            tokenObject = Resources.Load<TokenScriptableObject>("Tokens/" + name);
        }
        if (tokenObject.baseInitiation > 0)
        {
            GameObject initiationText = Instantiate(Resources.Load<GameObject>("Prefabs/InitiationText"));
            initiationText.name = "InitiationText";
            initiationText.GetComponent<Text>().text = tokenObject.baseInitiation.ToString();
            initiationText.transform.SetParent(transform.Find("Canvas").transform);
            initiationText.transform.localPosition = new Vector3(tokenObject.initiation1X, tokenObject.initiation1Y, -1.05f);
            initiationText.transform.localRotation = new Quaternion(0, 0, 0, 0);
            initiationText.transform.localScale = new Vector3(1, 1, 1);

            if (tokenObject.doubleAttack)
            {
                GameObject initiationText2 = Instantiate(Resources.Load<GameObject>("Prefabs/InitiationText"));
                initiationText2.name = "InitiationText2";
                initiationText2.GetComponent<Text>().text = (tokenObject.baseInitiation - 1).ToString();
                initiationText2.transform.SetParent(transform.Find("Canvas").transform);
                initiationText2.transform.localPosition = new Vector3(tokenObject.initiation2X, tokenObject.initiation2Y, -1.05f);
                initiationText2.transform.localRotation = new Quaternion(0, 0, 0, 0);
                initiationText2.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        health += tokenObject.baseArmor;
        initiation = tokenObject.baseInitiation;
        mobility = tokenObject.mobility;

        Material[] materials = GetComponent<MeshRenderer>().materials;
        materials[1] = Resources.Load<Material>("Materials/" + tokenObject.name);
        GetComponent<MeshRenderer>().materials = materials;
    }

    public void ResetPosition()
    {
        ResetPointOnBoardToken();
        i = 0;
        j = 0;
    }

    public void UpdateInitiationText()
    {
        if (transform.Find("Canvas").Find("InitiationText"))
        {
            transform.Find("Canvas").transform.Find("InitiationText").GetComponent<Text>().text = initiation.ToString();
            Color color;
            if (initiation > tokenObject.baseInitiation)
            {
                color = new Color(0, 100, 0);
            }
            else if (initiation < tokenObject.baseInitiation)
            {
                color = new Color(100, 0, 0);
            }
            else
            {
                color = new Color(255, 255, 255);
            }
            transform.Find("Canvas").transform.Find("InitiationText").GetComponent<Text>().color = color;
        }
        else
        {
            Debug.Log("Nie znaleziono");
        }
    }

    void ResetPointOnBoardToken()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("PointOnBoard");
        foreach (var point in points)
        {
            var script = point.GetComponent<PointOnBoardScript>();
            int pointI = script.gridI;
            int pointJ = script.gridJ;
            if (pointI == i)
            {
                if (pointJ == j)
                {
                    script.ResetToken();
                }
            }
        }
    }

    public void GetAttacked(int attack)
    {
        health -= attack;
        if(health < 1)
        {
            Destroy(this);
        }
    }

    public void SetPosition(int newI, int newJ)
    {
        i = newI;
        j = newJ;
    }
}
