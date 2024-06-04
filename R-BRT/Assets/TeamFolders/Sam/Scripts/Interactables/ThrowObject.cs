using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    [SerializeField] PickUpObject pickUpObject;
         
    [SerializeField] private float shootForce;      
    


    private void Start()
    {
        shootForce = 700.0f;
        pickUpObject = GetComponent<PickUpObject>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Rigidbody rb = pickUpObject.ReturnThisObject().GetComponent<Rigidbody>();

        

        Vector3 shootDirection = Camera.main.transform.forward;

        if (rb != null && pickUpObject.ReturnHoldingStatus() == true)
        {
            

            // Apply force to the projectile to shoot it

            
            rb.constraints = RigidbodyConstraints.None;
            pickUpObject.GetComponent<PickUpObject>().PutDown();
            rb.AddForce(shootDirection * shootForce);
        }
    }
}

