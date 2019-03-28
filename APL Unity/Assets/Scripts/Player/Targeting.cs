using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    public Camera mainCam;
    private Vector3 mousePos;
    private Vector2 screenPoint;

    private float angle;

    // public Transform target; //Assign to the object you want to rotate
    private Vector3 worldPos;
 
 void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = mainCam.transform.position.z;
        screenPoint = mainCam.WorldToScreenPoint(transform.position);

        mousePos.x -= screenPoint.x;
        mousePos.y -= screenPoint.y;

        angle = Mathf.Atan2(mousePos.y, mousePos.x)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
