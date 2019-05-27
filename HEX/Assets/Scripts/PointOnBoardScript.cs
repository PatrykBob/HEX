using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOnBoardScript : MonoBehaviour
{
    public int gridI;
    public int gridJ;

    public GameObject token;

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
