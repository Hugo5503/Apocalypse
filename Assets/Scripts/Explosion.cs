using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    private ParticleSystem partsystem;
	// Use this for initialization
	void Start () {
        partsystem = GetComponent<ParticleSystem>();
        StartCoroutine(waitToDestroy());
	}

    private IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

}
