using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialAimScript : MonoBehaviour, IInteractable
{

    public Transform tube;
    public float rotSpeed;

    public float minAngle;
    public float maxAngle;

    private bool rotating = false;
    private Vector3 mouseStartPos;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Stop closing if players releases e key.
        if (Input.GetKeyUp(KeyCode.E))
        {
            rotating = false;
        }

        //Check if close enough.
        if (rotating && Vector3.Distance(transform.position, Camera.main.transform.position) < 3)
        {
            Vector3 newMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            //look if mouseinput changed if there is little to no movement do nothing else move left or right
            if (mouseStartPos.y - newMousePos.y < 0.02f && (tube.rotation.eulerAngles.x >= minAngle))
            {
                tube.Rotate(this.transform.up * rotSpeed);
                transform.Rotate(Vector3.up * (rotSpeed * 75));
            }

            if (mouseStartPos.y - newMousePos.y > -0.02f && (tube.rotation.eulerAngles.x <= maxAngle))
            {
                tube.Rotate(this.transform.up * rotSpeed * -1);
                transform.Rotate(Vector3.up * (rotSpeed * 75) * -1);
            }
        }
        else
        {
            rotating = false;
        }
    }

    public void Activate()
    {
        mouseStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        rotating = true;
    }

    public void DeActivate()
    {
        rotating = false;
    }
}
