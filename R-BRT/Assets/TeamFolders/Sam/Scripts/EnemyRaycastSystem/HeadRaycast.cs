using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRaycast : MonoBehaviour
{
    [SerializeField] private float raycastDistance;
    [SerializeField] float interactDistance = 10;
    [SerializeField] Light enemyLight;


    // Start is called before the first frame update
    void Start()
    {
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
                    enemyLight.color = Color.red;
                }
            }
            else
            {
                enemyLight.color = Color.green;
            }
        }
    }
}

