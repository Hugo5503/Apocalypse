using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarGear : MonoBehaviour, IDestroyable, IDamageable<float> {

    public float hp = 200;

    private bool setup;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Damage(float damageTaken)
    {
        hp -= damageTaken;
        if(hp <= 0)
        {
            DestroyAble();
        }
    }

    public void DestroyAble()
    {
        throw new NotImplementedException();
    }
}
