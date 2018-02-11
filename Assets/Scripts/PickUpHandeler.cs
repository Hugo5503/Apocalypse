using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandeler : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public Transform leftHandPos;
    public Transform rightHandPos;

    private bool picking;

    enum leftRight
    {
        left, right
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            picking = true;
            Invoke("DisablePicking", 2);
        }

        //UsingLeft
        if (Input.GetMouseButtonDown(0))
        {

        }

        //Using right
        if(Input.GetMouseButtonDown(1))
        {

        }

        //PickingUpLeft
        if (Input.GetMouseButtonDown(0) && picking)
        {
            pickUp(leftRight.left);
            picking = false;
        }

        //PickingUpRight
        if (Input.GetMouseButtonDown(1) && picking)
        {
            pickUp(leftRight.right);
            picking = false;
        }
    }

    void pickUp(leftRight lr)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3))
        {
            MonoBehaviour[] scripts = hit.collider.gameObject.GetComponents<MonoBehaviour>();
            if (scripts.Length > 0)
            {
                foreach (MonoBehaviour script in scripts)
                {
                    if (script is IInteractable<GameObject>)
                    {
                        if (lr == leftRight.left)
                        {
                            if (leftHand != null)
                            {
                                DropObject(leftRight.left);
                            }
                            leftHand = placeObject(leftRight.left, hit.collider.gameObject);
                        }
                        else
                        {
                            if (rightHand != null)
                            {
                                DropObject(leftRight.right);
                            }
                            rightHand = placeObject(leftRight.right, hit.collider.gameObject);
                        }
                        return;
                    }

                    //if(specialcaseScript)
                    //Execute special case
                }
            }
            DropObject(lr);
            Debug.Log("Nothing hit but close close");
        }
        else
        {
            DropObject(lr);
        }
    }

    private void DisablePicking()
    {
        picking = false;
    }

    private GameObject placeObject(leftRight lr, GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        if (lr == leftRight.left)
        {
            obj.transform.position = leftHandPos.position;
            obj.transform.SetParent(leftHandPos);
        }

        if (lr == leftRight.right)
        {
            obj.transform.position = rightHandPos.position;
            obj.transform.SetParent(rightHandPos);
        }
        return obj;
    }

    private void DropObject(leftRight lr)
    {
        if (lr == leftRight.left)
        {
            leftHand.GetComponent<Rigidbody>().isKinematic = false;
            leftHand.transform.parent = null;
            leftHand = null;
        }
        if (lr == leftRight.right)
        {
            rightHand.GetComponent<Rigidbody>().isKinematic = false;
            rightHand.transform.parent = null;
            rightHand = null;
        }
    }
}
