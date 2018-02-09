using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialAim2Script : MonoBehaviour, IInteractable<GameObject>
{

    public Transform Mortar;
    public float rotSpeed = 0.3f;

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

            if (mouseStartPos.y - newMousePos.y < 0.02f)
            {
                Mortar.Rotate(Vector3.up * rotSpeed);
                transform.Rotate(Vector3.up * (rotSpeed * 24));
            }

            if (mouseStartPos.y - newMousePos.y > -0.02f)
            {
                Mortar.Rotate(Vector3.up * rotSpeed * -1);
                transform.Rotate(Vector3.up * (rotSpeed * 24) * -1);
            }
        }
    }

    public GameObject Interactable(GameObject interactable)
    {
        mouseStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        if (interactable == null)
        {
            rotating = true;
        }
        return null;
    }
}
