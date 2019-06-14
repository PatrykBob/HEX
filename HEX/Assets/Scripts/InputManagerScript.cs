using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManagerScript : MonoBehaviour
{
    public GameObject selectedToken;
    public GameObject tokenToSelect;
    public GameObject board;

    public bool moving;
    public bool rotating;

    public bool inPlace;

    public FractionEnum.Fraction fraction;

    void Update()
    {
        TouchControl();
        /*if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
            RaycastHit[] test = GetRaycasts();
            foreach(var hit in test)
            {
                Debug.Log(hit.transform.name);
            }
        }*/
    }

    RaycastHit[] GetRaycasts()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        return Physics.RaycastAll(ray);
    }

    void CheckIfTokenHit(RaycastHit[] hits)
    {
        bool rotatingHit = false;
        bool movingHit = false;

        foreach (var hit in hits)
        {
            if (hit.transform.gameObject.name == "RotationQuad" && hit.transform.parent.gameObject == selectedToken)
            {
                rotatingHit = true;
            }
            if (hit.transform.gameObject.name == "MovingQuad" && hit.transform.parent.gameObject == selectedToken)
            {
                movingHit = true;
                inPlace = false;
            }
        }
        if (movingHit)
        {
            moving = true;
            //selectedToken.GetComponent<TokenScript>().ResetPosition();
        }
        else if (rotatingHit && !movingHit)
        {
            rotating = true;
        }
    }

    void CheckBuffs()
    {
        Debug.Log("Check Input");
        GetComponent<PlayerScript>().CmdCheckBuffs();
    }

    void CheckTokenPlace(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.transform.tag == "PointOnBoard")
            {
                if (hit.transform.GetComponent<PointOnBoardScript>().token == null)
                {
                    selectedToken.transform.position = hit.transform.position;
                    //hit.transform.GetComponent<PointOnBoardScript>().ChangeToken(selectedToken);
                    inPlace = true;
                }
            }
        }
        moving = false;
        CheckBuffs();
    }

    void SnapTokenRotation()
    {
        Vector3 rot = new Vector3(270, 0, 0);
        float rotation = selectedToken.transform.eulerAngles.y;
        if (rotation >= 270 && rotation < 330)
        {
            rot.y = 300;
            selectedToken.transform.localRotation = Quaternion.Euler(rot);
            selectedToken.GetComponent<TokenScript>().rotation = 5;
        }
        else if (rotation >= 330 || rotation < 30)
        {
            rot.y = 0;
            selectedToken.transform.localRotation = Quaternion.Euler(rot);
            selectedToken.GetComponent<TokenScript>().rotation = 0;
        }
        else if (rotation >= 30 && rotation < 90)
        {
            rot.y = 60;
            selectedToken.transform.localRotation = Quaternion.Euler(rot);
            selectedToken.GetComponent<TokenScript>().rotation = 1;
        }
        else if (rotation >= 90 && rotation < 150)
        {
            rot.y = 120;
            selectedToken.transform.localRotation = Quaternion.Euler(rot);
            selectedToken.GetComponent<TokenScript>().rotation = 2;
        }
        else if (rotation >= 150 && rotation < 210)
        {
            rot.y = 180;
            selectedToken.transform.localRotation = Quaternion.Euler(rot);
            selectedToken.GetComponent<TokenScript>().rotation = 3;
        }
        else if (rotation >= 210 && rotation < 270)
        {
            rot.y = 240;
            selectedToken.transform.localRotation = Quaternion.Euler(rot);
            selectedToken.GetComponent<TokenScript>().rotation = 4;
        }
        rotating = false;
        CheckBuffs();
    }

    void TokenMoveRotate()
    {
        RaycastHit[] hits = GetRaycasts();

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckIfTokenHit(hits);
        }

        if (moving)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.name == "TokenHoverQuad")
                {
                    selectedToken.transform.position = hit.point;
                }
            }
        }

        if (rotating)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.name == "TokenRotationQuad")
                {
                    selectedToken.transform.rotation = Quaternion.LookRotation(hit.point) * Quaternion.Euler(-90, -90, 0);
                }
            }

        }

        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (moving)
            {
                CheckTokenPlace(hits);
            }
            else if (rotating)
            {
                SnapTokenRotation();
            }
        }
    }

    void TokenSelection()
    {
        RaycastHit[] hits = GetRaycasts();

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.name == "MovingQuad")
                {
                    if (hit.transform.parent.GetComponent<TokenScript>().tokenObject.fraction == fraction)
                    {
                        if (hit.transform.parent.GetComponent<TokenScript>().canBeMoved)
                        {
                            tokenToSelect = hit.transform.parent.gameObject;
                        }
                    }
                }
            }
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            foreach (var hit in hits)
            {
                if (hit.transform.name == "MovingQuad")
                {
                    if(tokenToSelect == hit.transform.parent.gameObject)
                    {
                        selectedToken = tokenToSelect;
                        selectedToken.transform.Find("RotationQuad").gameObject.SetActive(true);
                        return;
                    }
                }
            }
            tokenToSelect = null;
        }
    }

    void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1 && selectedToken != null)
            {
                TokenMoveRotate();
            }
            else if(Input.touchCount == 1 && selectedToken == null)
            {
                TokenSelection();
            }
        }
    }

    public void SetFraction(FractionEnum.Fraction fractionToSet)
    {
        fraction = fractionToSet;
    }

    private void OnGUI()
    {
        if (selectedToken != null)
        {
            if (inPlace)
            {
                if (GUI.Button(new Rect(100, 300, 50, 50), "OK"))
                {
                    selectedToken.transform.Find("RotationQuad").gameObject.SetActive(false);
                    selectedToken.transform.gameObject.GetComponent<TokenScript>().canBeMoved = false;
                    selectedToken = null;
                    CheckBuffs();
                }
            }
        }
    }
}
