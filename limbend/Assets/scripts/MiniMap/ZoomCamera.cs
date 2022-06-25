using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    public Camera mainCamera;

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

    Vector2 firstTouchPrevPos, secondTouchPrevPos;

    [SerializeField]
    float zoomModifierSpeed = 0.1f;


    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;

    bool multiTouch;

    private test g;
    private int size;

    void Start()
    {
        g = GameObject.Find("simulation").GetComponent<test>();
        size = g.size;

        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
       
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                multiTouch = false;
                hit_position = touch.position;
                    camera_position = transform.position;
                }
        }
        
        if (Input.touchCount == 2)
        {
            multiTouch = true;
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
                mainCamera.orthographicSize += zoomModifier;
            if (touchesPrevPosDifference < touchesCurPosDifference)
                mainCamera.orthographicSize -= zoomModifier;

        }

        if (Input.touchCount > 1)
        {
            multiTouch = true;
        }

        else if (Input.touchCount == 1 && multiTouch == false)
        {
            Touch touch = Input.GetTouch(0);
                current_position = touch.position;
                LeftMouseDrag();
        }

        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 20f, size / 2 + 50);
    }


    void LeftMouseDrag()
    {
        // From the Unity3D docs: "The z position is in world units from the camera."  In my case I'm using the y-axis as height
        // with my camera facing back down the y-axis.  You can ignore this when the camera is orthograhic.
        current_position.z = hit_position.z = camera_position.y;

        // Get direction of movement.  (Note: Don't normalize, the magnitude of change is going to be Vector3.Distance(current_position-hit_position)
        // anyways.  
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

        // Invert direction to that terrain appears to move with the mouse.
        direction = direction * -1;

        Vector3 position = camera_position + direction;

        transform.position = position;
    }
}

