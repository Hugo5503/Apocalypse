using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarShoot : MonoBehaviour
{
    public GameObject ShowShell;

    private float startTime;
    private float journeyLength;
    private bool lerping = false;
    private float speed = 0.5f;

    public Transform startMarker;
    public Transform endMarker;

    public GameObject explosion;
    public GameObject mortarRound;

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (lerping)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            ShowShell.transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
            speed += 0.1f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < 15)
        {
            if (collision.gameObject.name.Contains("Shell"))
            {
                Destroy(collision.gameObject);
                ShowShell.SetActive(true);
                StartCoroutine(shootProcess());
            }
        }
    }

    private IEnumerator shootProcess()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        startTime = Time.time;
        lerping = true;
        yield return new WaitForSeconds(1f);
        lerping = false;
        ShowShell.SetActive(false);
        ShowShell.transform.position = startMarker.transform.position;
        speed = 0.5f;
        explosion.SetActive(true);
        GameObject round = Instantiate(mortarRound, transform.position, GetComponentInParent<Transform>().rotation);
        MortarRound roundScript = round.GetComponent<MortarRound>();
        roundScript.LiveRound();
        yield return new WaitForSeconds(.5f);
        explosion.SetActive(false);
        collider.enabled = true;

    }
}
