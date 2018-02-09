﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject holdingObject;
    public FixedJoint holdingJoint;
    public GameObject holdPos;
    public GameObject crowbar;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (transform.GetChild(0).gameObject.activeSelf)
            {
                DropObject();
            }
            if (holdingObject == null)
                tryInteract();
            else
                DropObject();
        }
    }

    private void tryInteract()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3))
        {
            MonoBehaviour[] scripts = hit.collider.gameObject.GetComponents<MonoBehaviour>();
            if (scripts.Length > 0)
            {
                foreach (MonoBehaviour script in scripts)
                {
                    if (script is IInteractable<GameObject>)
                    {
                        IInteractable<GameObject> interactable = (IInteractable<GameObject>)script;
                        GameObject returnValue = interactable.Interactable(holdingObject);
                        if (returnValue != null)
                        {
                            if (hit.collider.gameObject.name.Contains("Crowbar"))
                            {
                                Destroy(hit.collider.gameObject);
                                Crowbar();
                            }

                            else
                                PickUp(returnValue);
                        }
                    }
                }
            }
        }
    }

    private void PickUp(GameObject returnValue)
    {
        holdingObject = returnValue;
        holdingObject.transform.position = holdPos.transform.position;
        holdingJoint.connectedBody = holdingObject.GetComponent<Rigidbody>();
        Physics.IgnoreCollision(this.GetComponentInParent<Collider>(), holdingObject.GetComponent<Collider>(), true);
    }

    private void DropObject()
    {
        if (transform.GetChild(0).gameObject.name.Contains("Crowbar"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            Instantiate(crowbar, this.transform.position, new Quaternion());
            return;
        }
        holdingJoint.connectedBody = null;
        Physics.IgnoreCollision(this.GetComponentInParent<Collider>(), holdingObject.GetComponent<Collider>(), false);
        holdingObject = null;
    }

    private void Crowbar()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}