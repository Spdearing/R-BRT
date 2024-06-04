using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    [SerializeField] PickUpObject pickUpObject;
         
    [SerializeField] private float shootForce;
    [SerializeField] private float upwardForce;
    


    private void Start()
    {
        shootForce = 10.0f;
        upwardForce = 5.0f;
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
        GameObject heldObject = pickUpObject.ReturnThisObject();
        Rigidbody rb = pickUpObject.ReturnThisObject().GetComponent<Rigidbody>();
        
        Vector3 shootDirection = Camera.main.transform.forward;

        if (rb != null && pickUpObject.ReturnHoldingStatus() == true)
        {
            
            rb.constraints = RigidbodyConstraints.None;
            //heldObject.transform.SetParent(null);
           
            pickUpObject.GetComponent<PickUpObject>().PutDown();
            rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
            rb.AddForce(shootDirection * upwardForce);
        }
    }
}

