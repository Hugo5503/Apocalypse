using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour, IInteractable<GameObject>
{

    [Range(0, 100)]
    public int magsize;

    [Range(0, 100)]
    public int bulletCount;

    public Transform[] bullets;
    public bool fire;
    public bool reload;

    // Use this for initialization
    void Start()
    {
        fire = false;

        bullets = GetComponentsInChildren<Transform>();
        int parentId = System.Array.IndexOf(bullets, transform);
        Array.Reverse(bullets);
        Array.Resize(ref bullets, bullets.Length - 1);
        Array.Reverse(bullets);
    }

    // Update is called once per frame
    void Update()
    {
        if (fire)
        {
            if (bulletCount > 0)
                Fire();

            fire = false;
        }
        if (reload)
        {
            if (bulletCount < magsize)
                Reload();
            reload = false;
        }
    }

    bool Reload()
    {
        if (bulletCount < magsize)
        {
            bullets[bulletCount].gameObject.SetActive(true);
            bulletCount++;
            return true;
        }
        else
        {
            return false;
        }
    }

    bool Fire()
    {
        if (bulletCount > 0)
        {
            bullets[bulletCount - 1].gameObject.SetActive(false);
            bulletCount--;
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject Interactable(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}
