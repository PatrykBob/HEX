using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TokenScript : NetworkBehaviour
{
    public int i;
    public int j;

    public TokenScriptableObject tokenObject;
    // Start is called before the first frame update
    void Start()
    {
        if (tokenObject.initiation > 0)
        {
            GameObject initiationText = Instantiate(Resources.Load<GameObject>("Prefabs/InitiationText"));
            initiationText.GetComponent<Text>().text = tokenObject.initiation.ToString();
            initiationText.transform.SetParent(transform.FindChild("Canvas").transform);
            initiationText.transform.localPosition = new Vector3(tokenObject.initiation1X, tokenObject.initiation1Y, -1.05f);
            initiationText.transform.localRotation = new Quaternion(0, 0, 0, 0);
            initiationText.transform.localScale = new Vector3(1, 1, 1);

            if (tokenObject.doubleAttack)
            {
                GameObject initiationText2 = Instantiate(Resources.Load<GameObject>("Prefabs/InitiationText"));
                initiationText2.GetComponent<Text>().text = (tokenObject.initiation - 1).ToString();
                initiationText2.transform.SetParent(transform.FindChild("Canvas").transform);
                initiationText2.transform.localPosition = new Vector3(tokenObject.initiation2X, tokenObject.initiation2Y, -1.05f);
                initiationText2.transform.localRotation = new Quaternion(0, 0, 0, 0);
                initiationText2.transform.localScale = new Vector3(1, 1, 1);
            }
        }
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

    void ResetPointOnBoardToken()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("PointOnBoard");
        foreach(var point in points)
        {
            var script = point.GetComponent<PointOnBoardScript>();
            int pointI = script.gridI;
            int pointJ = script.gridJ;
            if(pointI == i)
            {
                if(pointJ == j)
                {
                    script.ResetToken();
                }
            }
        }
    }

    public void SetPosition(int newI, int newJ)
    {
        i = newI;
        j = newJ;
    }
    // Update is called once per frame
    void Update()
    {
           
    }
}
