using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    //pc
    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 24f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraDisabled = false;

    //phone
    private Vector2 fingerStartPos = Vector2.zero;
    private bool isSwipe = false;

    public float PhoneSensitivity = 0.4f;
    public float PhoneScrollSensitvity = 0.2f;


    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
    }

    void LateUpdate()
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isSwipe = true;
                        fingerStartPos = touch.position;
                        break;

                    case TouchPhase.Canceled:
                        isSwipe = false;
                        break;

                    case TouchPhase.Ended:
                        isSwipe = false;
                        break;

                    case TouchPhase.Moved:
                        float gestureDist = (touch.position - fingerStartPos).magnitude;

                        Vector2 direction = touch.position - fingerStartPos;
                        Vector2 swipeType = Vector2.zero;

                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                        {
                            // the swipe is horizontal:
                            swipeType = Vector2.right * Mathf.Sign(direction.x);
                        }
                        else
                        {
                            // the swipe is vertical:
                            swipeType = Vector2.up * Mathf.Sign(direction.y);
                        }

                        if (swipeType.x != 0.0f)
                        {
                            if (swipeType.x > 0.0f)
                            {
                                _LocalRotation.x += (gestureDist * MouseSensitivity)/600;
                            }
                            else
                            {
                                _LocalRotation.x -= (gestureDist * MouseSensitivity)/600;
                            }
                        }

                        if (swipeType.y != 0.0f)
                        {
                            if (swipeType.y > 0.0f)
                            {
                                _LocalRotation.y -= (gestureDist * MouseSensitivity) / 400;
                                if (_LocalRotation.y < 30f)
                                    _LocalRotation.y = 30f;
                            }
                            else
                            {
                                _LocalRotation.y += (gestureDist * MouseSensitivity) / 400;
                                if (_LocalRotation.y > 90f)
                                    _LocalRotation.y = 90f;
                            }
                        }
                        break;
                }
                Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
                this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

                if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
                {
                    this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
                }
            }
        }
    }

    void HandleMouse()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
            _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;

            if (_LocalRotation.y < 30f)
                _LocalRotation.y = 30f;
            else if (_LocalRotation.y > 90f)
                _LocalRotation.y = 90f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;
            ScrollAmount *= (this._CameraDistance * 0.3f);
            this._CameraDistance += ScrollAmount * -1f;
            this._CameraDistance = Mathf.Clamp(this._CameraDistance, 10f, 50f);
        }

        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }
}
