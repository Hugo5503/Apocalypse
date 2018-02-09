using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour, IDamageable<int>, IDestroyable {

    [Range(0,100)]
    private int health;

    // Use this for initialization
    void Start () {
        health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Damage(int damageTaken)
    {
        health -= damageTaken;
        if(health <= 0)
        {
            DestroyAble();
        }
    }

    public void DestroyAble()
    {
        Destroy(gameObject);
    }
}
