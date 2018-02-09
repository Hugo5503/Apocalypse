using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableScript : MonoBehaviour, IInteractable<GameObject> {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject Interactable(GameObject pickUpObject)
    {
        GameObject objecttry = null;
        if(pickUpObject == null)
        {
            objecttry = this.gameObject;
        }
        return objecttry;
    }
}
