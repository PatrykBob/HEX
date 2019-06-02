using System;
using System.Collections;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    float prevDistance;
    float zoomSpeed = 1.5f;
    Vector2 prevPosOne;
    Vector2 prevPosTwo;
    Vector2 swipeDirectionOne;
    Vector2 swipeDirectionTwo;
    Vector2 secondPressPosOne;
    Vector2 secondPressPosTwo;
    //bool isPinching;
    public Transform Target;
    //public GameObject Player, Y_Yaw, X_Pitch;

    void Update()
    {
        if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began))
        {
            prevPosOne = Input.GetTouch(0).position;
            prevPosTwo = Input.GetTouch(1).position;

            prevDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
        }
        else if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
        {
            float distance;
            

            Vector2 touch1 = Input.GetTouch(0).position;
            Vector2 touch2 = Input.GetTouch(1).position;

            distance = Vector2.Distance(touch1, touch2);

            secondPressPosOne = new Vector2(touch1.x, touch1.y);
            secondPressPosTwo = new Vector2(touch2.x, touch2.y);

            swipeDirectionOne = new Vector3(secondPressPosOne.x - prevPosOne.x, secondPressPosOne.y - prevPosOne.y);
            swipeDirectionTwo = new Vector3(secondPressPosTwo.x - prevPosTwo.x, secondPressPosTwo.y - prevPosTwo.y);

            swipeDirectionOne.Normalize();
            swipeDirectionTwo.Normalize();

            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            if (swipeDirectionOne.y > 0 & swipeDirectionOne.x > -0.5f & swipeDirectionOne.x < 0.5f
                & swipeDirectionTwo.y > 0 & swipeDirectionTwo.x > -0.5f & swipeDirectionTwo.x < 0.5f
                || swipeDirectionOne.y < 0 & swipeDirectionOne.x > -0.5f & swipeDirectionOne.x < 0.5f
                & swipeDirectionTwo.y < 0 & swipeDirectionTwo.x > -0.5f & swipeDirectionTwo.x < 0.5f
                || swipeDirectionOne.x < 0 & swipeDirectionOne.y > -0.5f & swipeDirectionOne.y < 0.5f
                & swipeDirectionTwo.x < 0 & swipeDirectionTwo.y > -0.5f & swipeDirectionTwo.y < 0.5f
                || swipeDirectionOne.x > 0 & swipeDirectionOne.y > -0.5f & swipeDirectionOne.y < 0.5f
                & swipeDirectionTwo.x > 0 & swipeDirectionTwo.y > -0.5f & swipeDirectionTwo.y < 0.5f)
            {
                Camera.main.transform.RotateAround(Target.transform.position,Camera.main.transform.right, -touchDeltaPosition.y * 5.0f * Time.deltaTime);
                Camera.main.transform.RotateAround(Target.transform.position,Vector3.up, touchDeltaPosition.x * 5.0f * Time.deltaTime);
            }
            else
            {
                float pichAmount = (distance - prevDistance) * zoomSpeed * Time.deltaTime;
                Camera.main.transform.Translate(0, 0, pichAmount);
                prevDistance = distance;
            }
        }
        transform.LookAt(Target.transform);
    }
}