using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{

    [Header("Scripts")]
    [SerializeField] private PickUpObject pickUpObject;


    [Header("Floats")]
    [SerializeField] private float shootForce;
    [SerializeField] private float upwardForce;

    [Header("Bools")]
    [SerializeField] private bool threwObject;

    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;



    private void Start()
    {
        threwObject = false;
        shootForce = 10.0f;
        upwardForce = 5.0f;
        pickUpObject = GetComponent<PickUpObject>();
        playerAnimator = GameObject.FindWithTag("Body").GetComponent<Animator>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            threwObject = true;
            StartCoroutine(ResetThrowStatus());
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
            playerAnimator.SetBool("holdingRock", false);
            playerAnimator.SetBool("pickingUpRock", false);
        }
    }

    IEnumerator ResetThrowStatus()
    {
        yield return new WaitForSeconds(3.0f);
        threwObject = false;
    }

    public bool ReturnThrowStatus()
    {
        return this.threwObject;
    }
}

