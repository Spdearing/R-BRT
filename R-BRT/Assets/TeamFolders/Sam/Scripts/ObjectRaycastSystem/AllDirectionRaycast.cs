using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDirectionRaycast : MonoBehaviour
{
    public int numRays = 100; // Number of rays to cast
    public float raycastDistance = 20f; // Distance for the raycasts
    public LayerMask layerMask; // Layer mask to filter raycast hits

    [SerializeField] private GameObject enemyObject;

    void Update()
    {
        //CastRaysInAllDirections();
    }

    public void CastRaysInAllDirections()
    {
        for (int i = 0; i < numRays; i++)
        {
            // Generate a random direction
            Vector3 direction = Random.onUnitSphere;

            // Cast a ray in this direction
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;

            // Debug: Draw the ray in the editor
            Debug.DrawRay(transform.position, direction * raycastDistance, Color.red);

            if (Physics.Raycast(ray, out hit, raycastDistance, layerMask))
            {
               if(hit.collider.tag == "GroundBot")
                {
                    enemyObject = hit.collider.gameObject;
                    enemyObject.transform.LookAt(transform.position);
                }
            }
        }
    }
}

