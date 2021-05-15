using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    public int checkMagnitude = 125;
    private bool tap;
    private bool isDraging;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;
    private Vector2 startTouch;
    private Vector2 swipeDelta;

    private void ListenToStandaloneInputs()
    {
        if (Input.GetMouseButtonDown(0)) {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Reset();
        }
    }

    private void ListenToMobileInputs()
    {
        if (Input.touches.Length > 0) {
            if (Input.touches[0].phase.Equals(TouchPhase.Began)) {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase.Equals(TouchPhase.Ended) || Input.touches[0].phase.Equals(TouchPhase.Canceled))
            {
                Reset();
            }

        }
    
        swipeDelta = Vector2.zero;

        if (isDraging) {
            if (Input.touches.Length > 0) {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        if (swipeDelta.magnitude > checkMagnitude) {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y)) {
                swipeRight = !(swipeLeft = x < 0);
            }
            else
            {
                swipeUp = !(swipeDown = y < 0);   
            }

            Reset();
        }
    }

    private void Update()
    {
        tap = swipeDown = swipeUp = swipeRight = swipeLeft = false;
        ListenToStandaloneInputs();
        ListenToMobileInputs();
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }

    public Vector2 SwipeDelta { get { return swipeDelta; } }

    public bool Tap { get {return this.tap;} }

    public bool SwipeLeft { get { return swipeLeft; } }

    public bool SwipeRight { get { return swipeRight; } }

    public bool SwipeUp { get { return swipeUp; } } 

    public bool SwipeDown { get { return swipeDown; } } 
}
