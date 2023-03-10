using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseShoot : MonoBehaviour
{
    public Camera playerCamera;
    public AudioClip[] shootingSound = new AudioClip[4];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;

            // Do something with the object that was hit by the raycast.
            Debug.DrawLine(ray.origin, hit.point);
            if (Input.GetMouseButtonDown(0))
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().totalShots += 1;
                GameObject.Find("Main Camera").GetComponent<AudioSource>().PlayOneShot(shootingSound[Random.Range(0, 3)]);
                if (objectHit.tag == "Target")
                {
                    Destroy(objectHit);
                    GameObject.Find("GameManager").GetComponent<GameManager>().targetsDestroyed += 1;
                    if (GameObject.Find("GameManager").GetComponent<GameManager>().gameMode == "Instant")
                    {
                        GameObject.Find("GameManager").GetComponent<GameManager>().spawnTarget();
                    }
                }
            }
        }
    }
}
