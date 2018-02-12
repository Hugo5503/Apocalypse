using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandeler : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public Transform leftHandPos;
    public Transform rightHandPos;
    public Transform holdPos;

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
        if (Input.GetMouseButtonDown(0) && leftHand != null)
        {
            UseHoldingObject(leftRight.left);
        }

        //Using right
        if (Input.GetMouseButtonDown(1) && rightHand != null)
        {
            UseHoldingObject(leftRight.right);
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
                    if (script is IPickable)
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

                    if(script is IInteractable)
                    {
                        IInteractable localScript = (IInteractable)script;
                        localScript.Activate();
                    }



                    //if(specialcaseScript)
                    //Execute special case
                }
            }

            DropObject(lr);
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
        if (lr == leftRight.left && leftHand != null)
        {
            leftHand.GetComponent<Rigidbody>().isKinematic = false;
            leftHand.transform.parent = null;
            leftHand = null;
        }
        if (lr == leftRight.right && rightHand != null)
        {
            rightHand.GetComponent<Rigidbody>().isKinematic = false;
            rightHand.transform.parent = null;
            rightHand = null;
        }
    }

    private void UseHoldingObject(leftRight lr)
    {
        if (lr == leftRight.left && leftHand != null)
        {
            leftHand.transform.position = holdPos.position;
            leftHand.GetComponent<Rigidbody>().useGravity = false;
            leftHand.GetComponent<Rigidbody>().isKinematic = false;
        }
        if (lr == leftRight.right && rightHand != null)
        {
            rightHand.transform.position = holdPos.position;
            rightHand.GetComponent<Rigidbody>().useGravity = false;
            rightHand.GetComponent<Rigidbody>().isKinematic = false;
        }
        StartCoroutine(retunToHand(lr));
    }

    IEnumerator retunToHand(leftRight lr)
    {
        yield return new WaitForSeconds(1f);
        if (lr == leftRight.left && leftHand != null)
        {
            leftHand.GetComponent<Rigidbody>().useGravity = true;
            leftHand.GetComponent<Rigidbody>().isKinematic = true;
            leftHand.transform.position = leftHandPos.position;
        }
        if (lr == leftRight.right && rightHand != null)
        {
            leftHand.GetComponent<Rigidbody>().useGravity = true;
            leftHand.GetComponent<Rigidbody>().isKinematic = true;
            rightHand.transform.position = rightHandPos.position;
        }
    }
}
