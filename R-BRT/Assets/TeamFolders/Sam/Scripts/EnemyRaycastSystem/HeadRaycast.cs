using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRaycast : MonoBehaviour
{
    [SerializeField] private float raycastDistance;
    [SerializeField] float interactDistance = 10;
    [SerializeField] Light enemyLight;
    [SerializeField] private GameObject player;
    [SerializeField] private HeadMovement headMovement;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        headMovement = GetComponent<HeadMovement>();
        raycastDistance = interactDistance;
        enemyLight = GetComponentInChildren<Light>();
        enemyLight.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            string objectHit = hitInfo.collider.gameObject.tag;

            Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red);

            if (hitInfo.distance < interactDistance)
            {
                if (objectHit == "Player")
                {
                    headMovement.SetPlayerSpotted(true);
                    transform.LookAt(player.transform.position);
                    enemyLight.color = Color.red;
                }
                else if (objectHit != "player")
                {
                    headMovement.SetPlayerSpotted(false);
                    enemyLight.color = Color.green;
                }
            }
            else 

            headMovement.SetPlayerSpotted(false);
        }
    }
}

