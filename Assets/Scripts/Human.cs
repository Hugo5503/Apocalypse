using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour, IDamageable<int>, IDestroyable {

    [Range(0, 100)]
    private int health;
    public GameObject goal;
    private GameObject newGoal;

    // Use this for initialization
    void Start()
    {
        health = 100;
        Invoke("NewGoal",2);
    }

    // Update is called once per frame
    void Update()
    {
        if (newGoal != null)
        {
            transform.LookAt(newGoal.transform);
            GetComponent<Rigidbody>().AddForce(transform.forward * 10);
        }
    }

    public void Damage(int damageTaken)
    {
        health -= damageTaken;
        if (health <= 0)
        {
            DestroyAble();
        }
    }

    public void DestroyAble()
    {
        Destroy(gameObject);
    }

    private void NewGoal()
    {
        //Needs improving
        float randomX = Random.Range(this.transform.position.x - 10f, this.transform.position.x + 10f);
        float randomZ = Random.Range(this.transform.position.z - 10f, this.transform.position.z + 10f);
        if(newGoal != null)
        {
            Destroy(newGoal);
        }
        newGoal = Instantiate(goal);
        newGoal.transform.position = new Vector3(randomX,transform.position.y,randomZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == newGoal)
        {
            NewGoal();
        }
    }
}
