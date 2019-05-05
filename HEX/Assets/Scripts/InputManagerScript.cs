using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SelectedToken;

    public bool moving;
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            if(Input.touchCount == 1 && SelectedToken != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit[] hits = Physics.RaycastAll(ray);

                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.transform.gameObject == SelectedToken)
                        {
                            moving = true;
                        }
                    }
                }

                if (moving)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.transform.name == "TokenHoverQuad")
                        {
                            SelectedToken.transform.position = hit.point;
                        }
                    }
                }
                if(Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (moving)
                    {
                        foreach (var hit in hits)
                        {
                            if (hit.transform.tag == "PointOnBoard")
                            {
                                SelectedToken.transform.position = hit.transform.position;
                            }
                        }
                    }
                    moving = false;
                }
            }
        }   
    }
}
