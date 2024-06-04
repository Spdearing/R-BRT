using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private bool holding;
    private bool pickingUp;
    Rigidbody rb;

    [SerializeField] private Transform holdPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "PickUpItem";
        holdPosition = GameObject.Find("HoldPosition").GetComponentInChildren<Transform>();
        holding = false;
    }

    void Update()
    {
        if (pickingUp)
        {
            StartCoroutine(MoveObjectSmoothly(this.gameObject.transform.position, holdPosition.transform.position, 0.5f));
        }
        else if (holding)
        {
            StartCoroutine(MoveObjectSmoothly(this.gameObject.transform.position, holdPosition.transform.position, 0.05f));
            //rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void PickUp()
    {
        pickingUp = true;
        Holding();
    }

    void Holding()
    {
        pickingUp = false;
        holding = true;
    }

    public void PutDown()
    {
        holding = false;
        //rb.constraints = RigidbodyConstraints.None;
    }

    IEnumerator MoveObjectSmoothly(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; 
            transform.position = Vector3.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        transform.position = end;
    }

    public GameObject ReturnThisObject()
    {
        return this.gameObject;
    }

    public bool ReturnHoldingStatus()
    {
        return this.holding;
    }


}
