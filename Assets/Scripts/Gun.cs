using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject magInserted;
    public Transform magInsertPoint;
    public Transform magLoadedPoint;

    public GameObject gunSlide;
    public Transform gunSlideNor;
    public Transform gunSlideFar;

    public Magazine insertedMagazineData;
    public ParticleSystem muzzleFlash;
    public GameObject impactBullets;
    public float range;
    public float impactForce;
    public float minDamage;
    public float maxDamage;

    private bool lerpingMag = false;
    private bool lerpingSlideBack = false;
    private bool lerpingSlideForward = false;
    private float startTime;
    private float journeyLengthMag;
    private float journeyLengthSlide;
    private float speed = 0.01f;

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;

        journeyLengthMag = Vector3.Distance(magLoadedPoint.transform.position, magInsertPoint.position);

        journeyLengthSlide = Vector3.Distance(gunSlideNor.transform.position, gunSlideFar.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (lerpingMag == false && lerpingSlideBack == false && lerpingSlideForward == false)
            {
                ReloadAnimation();
                lerpingMag = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("fire");
            if (lerpingMag == false && lerpingSlideBack == false && lerpingSlideForward == false)
            {
                Fire();
                FireAnimation();
                lerpingSlideBack = true;
            }
        }

        if (lerpingMag)
        {
            if (!lerping(magInserted, magInsertPoint, magLoadedPoint, 15, 0))
            { lerpingMag = false; }
        }

        if (lerpingSlideBack)
        {
            if (!lerping(gunSlide, gunSlideNor, gunSlideFar, 200, 20))
            {
                lerpingSlideBack = false;
                startTime = Time.time;
                lerpingSlideForward = true;
            }
        }

        if (lerpingSlideForward)
        {
            if (!lerping(gunSlide, gunSlideFar, gunSlideNor, 50, 0))
            {
                lerpingSlideForward = false;
            }
        }
    }

    private void ReloadAnimation()
    {
        speed = 5f;
        magInserted.transform.position = magInsertPoint.position;
        magInserted.SetActive(true);
        startTime = Time.time;
        lerpingMag = true;
    }

    private void FireAnimation()
    {
        speed = 100;
        startTime = Time.time;
        lerpingSlideBack = true;
    }

    private void Fire()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            MonoBehaviour[] scripts = hit.collider.gameObject.GetComponents<MonoBehaviour>();
            if (scripts.Length > 0)
            {
                foreach (MonoBehaviour script in scripts)
                {
                    if (script is IDamageable<int>)
                    {
                        int damage = (int)System.Math.Truncate(UnityEngine.Random.Range(minDamage, maxDamage));
                        IDamageable<int> damagable = (IDamageable<int>)script;
                        damagable.Damage(damage);
                    }
                }
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject bulletImpact = Instantiate(impactBullets, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(bulletImpact, 2);
        }
    }

    private bool lerping(GameObject target, Transform a, Transform b, float localSpeed, float accelerate)
    {
        float distCovered = (Time.time - startTime) * localSpeed * Time.deltaTime;
        float fracJourney = distCovered / journeyLengthMag;
        target.transform.position = Vector3.Lerp(a.position, b.position, fracJourney);
        if (accelerate != 0)
            localSpeed += accelerate;
        if (target.transform.position == b.position)
        {
            return false;
        }
        return true;
    }
}
