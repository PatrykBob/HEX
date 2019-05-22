using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOnBoardScript : MonoBehaviour
{
    public int gridI;
    public int gridJ;

    public GameObject token;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToken(GameObject newToken)
    {
        token = newToken;
        if(token != null)
        {
            token.GetComponent<TokenScript>().SetPosition(gridI, gridJ);
        }
    }

    public void ResetToken()
    {
        token = null;
    }
}
