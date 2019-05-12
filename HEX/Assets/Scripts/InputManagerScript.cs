using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManagerScript : MonoBehaviour
{
    public GameObject SelectedToken;

    public bool moving;
    public bool rotating;
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
                    bool rotatingHit = false;
                    bool movingHit = false;

                    foreach (var hit in hits)
                    {
                        if (hit.transform.gameObject.name == "RotationQuad")
                        {
                            rotatingHit = true;
                        }
                        if (hit.transform.gameObject.name == "MovingQuad")
                        {
                            movingHit = true;
                        }
                    }
                    if (movingHit)
                    {
                        moving = true;
                    }
                    else if (rotatingHit && !movingHit)
                    {
                        rotating = true;
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

                if (rotating)
                {
                    foreach (var hit in hits)
                    {
                        if (hit.transform.name == "TokenRotationQuad")
                        {
                            SelectedToken.transform.rotation = Quaternion.LookRotation(hit.point) * Quaternion.Euler(-90, -90,0);
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
                        moving = false;
                    }
                    else if (rotating)
                    {
                        Vector3 rot = new Vector3(270, 0, 0);
                        float rotation = SelectedToken.transform.eulerAngles.y;
                        if (rotation >= 240 && rotation < 300)
                        {
                            rot.y = 270;
                            SelectedToken.transform.localRotation = Quaternion.Euler(rot);
                        }
                        else if (rotation >= 300 && rotation < 360)
                        {
                            rot.y = 330;
                            SelectedToken.transform.localRotation = Quaternion.Euler(rot);
                        }
                        else if (rotation >= 0 && rotation < 60)
                        {
                            rot.y = 30;
                            SelectedToken.transform.localRotation = Quaternion.Euler(rot);
                        }
                        else if (rotation >= 60 && rotation < 120)
                        {
                            rot.y = 90;
                            SelectedToken.transform.localRotation = Quaternion.Euler(rot);
                        }
                        else if (rotation >= 120 && rotation < 180)
                        {
                            rot.y = 150;
                            SelectedToken.transform.localRotation = Quaternion.Euler(rot);
                        }
                        else if (rotation >= 180 && rotation < 240)
                        {
                            rot.y = 210;
                            SelectedToken.transform.localRotation = Quaternion.Euler(rot);
                        }
                        rotating = false;
                    }
                }
            }
        }
        //For testing in PC
        //if (Input.GetMouseButton(0))
        //{
        //    if (!rotating)
        //    {
        //        Debug.Log(SelectedToken.transform.localEulerAngles);
        //    }
        //    rotating = true;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit[] hits = Physics.RaycastAll(ray);
        //    if (rotating)
        //    {
        //        foreach (var hit in hits)
        //        {
        //            if (hit.transform.name == "TokenRotationQuad")
        //            {
        //                SelectedToken.transform.rotation = Quaternion.LookRotation(hit.point) * Quaternion.Euler(-90, -90, 0);
        //            }
        //        }

        //    }
        //}
        //else
        //{
        //    float rotation = SelectedToken.transform.eulerAngles.y;
        //    Vector3 rot = new Vector3(270, 0, 0);
        //    if(rotation >= 240 && rotation < 300)
        //    {
        //        rot.y = 270;
        //        SelectedToken.transform.localRotation = Quaternion.Euler(rot);
        //    }
        //    else if(rotation >= 300 && rotation < 360)
        //    {
        //        rot.y = 330;
        //        SelectedToken.transform.localRotation = Quaternion.Euler(rot);
        //    }
        //    else if (rotation >= 0 && rotation < 60)
        //    {
        //        rot.y = 30;
        //        SelectedToken.transform.localRotation = Quaternion.Euler(rot);
        //    }
        //    else if (rotation >= 60 && rotation < 120)
        //    {
        //        rot.y = 90;
        //        SelectedToken.transform.localRotation = Quaternion.Euler(rot);
        //    }
        //    else if (rotation >= 120 && rotation < 180)
        //    {
        //        rot.y = 160;
        //        SelectedToken.transform.localRotation = Quaternion.Euler(rot);
        //    }
        //    else if (rotation >= 180 && rotation < 240)
        //    {
        //        rot.y = 210;
        //        SelectedToken.transform.localRotation = Quaternion.Euler(rot);
        //    }
        //    rotating = false;
        //}
    }
}
