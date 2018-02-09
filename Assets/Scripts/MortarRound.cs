using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarRound : MonoBehaviour, IDestroyable, IDamageable<float>
{

    public float force = 40f;
    public float radius = 5f;
    public float impactValue;
    public float explosionForce = 40f;

    public float minDamage = 50;
    public float maxDamage = 200;

    public GameObject explosion;
    //public GameObject smokeEffect;
    public bool liveRound = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (liveRound)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * force, ForceMode.VelocityChange);
            liveRound = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //smokeEffect.SetActive(false);
        if (collision.relativeVelocity.magnitude > 15)
        {
            Explode();
        }
    }

    public void LiveRound()
    {
        liveRound = true;
        //smokeEffect.SetActive(true);
    }

    public void Explode()
    {
        Instantiate(explosion, this.transform.position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-180, 180), 0)));

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (col != GetComponent<Collider>())
            {              
                MonoBehaviour[] scripts = col.GetComponents<MonoBehaviour>();
                if (scripts.Length > 0)
                {
                    foreach (MonoBehaviour script in scripts)
                    {
                        if (script is IDamageable<int>)
                        {
                            int damage = (int) Math.Truncate(UnityEngine.Random.Range(minDamage, maxDamage));
                            IDamageable<int> damagable = (IDamageable<int>)script;
                            damagable.Damage(damage);
                        }
                    }
                }

                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, radius);
                }
            }
            else
            {
                rb.detectCollisions = false;
            }
        }

        DestroyAble();
    }

    public void DestroyAble()
    {
        Destroy(gameObject);
    }

    public void Damage(float damageTaken)
    {
        if (damageTaken > 0)
            Explode();
    }
}
