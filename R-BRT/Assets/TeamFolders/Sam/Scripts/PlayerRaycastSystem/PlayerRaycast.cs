using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] private float raycastDistance;
    [SerializeField] float interactDistance = 4;
    [SerializeField] GameObject lastHitObject;
    //[SerializeField] GameObject enemy;
    //[SerializeField] Light enemyLight;
 
    [SerializeField] bool holding;
    [SerializeField] GameObject heldObject;
    [SerializeField] float pickUpCooldown = 0.5f;
    [SerializeField] float pickUpTime;

    [SerializeField] PickUpObject pickUpObject;
    [SerializeField] Transform holdingPosition;
    [SerializeField] GameObject heldPosition;
    

    //[SerializeField] ChangeBoxColor changeBoxColor;
    [SerializeField] TMP_Text interactableText;


    //[SerializeField] string[] tagsToCheck = new string[] { "Block", "Block2", "Block3", "PickUpItem" };



    void Start()
    {
        raycastDistance = interactDistance; // raycastDistance = 4
        holding = false;
        interactableText = GameObject.Find("InteractableText").GetComponent<TMP_Text>();
        pickUpObject = GameObject.Find("Rocks").GetComponent<PickUpObject>();
        holdingPosition = GameObject.Find("HoldPosition").GetComponent<Transform>();
        heldPosition = GameObject.Find("HoldPosition");

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
                Debug.Log("Hit Something");

                if(hitInfo.collider.tag == "PickUpItem")
                {
                    interactableText.text = "Press (F) to pick up the rock";

                    if (Input.GetKeyDown(KeyCode.F) && !holding)
                    {
                        holding = true;
                        hitInfo.collider.gameObject.GetComponent<PickUpObject>().PickUp();
                        heldObject = hitInfo.collider.gameObject;
                        pickUpTime = 0f;
                    }
                }
            }
            else
            {
                interactableText.text = " ";
                
            }

            pickUpTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.F) && holding && pickUpTime >= pickUpCooldown)
            {
                holding = false;
                heldObject.GetComponent<PickUpObject>().PutDown();
                heldObject = null;
            }
        }
    }


    //bool ContainsTag(string tag)
    //{
    //    foreach (string t in tagsToCheck)
    //    {
    //        if (t == tag)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}


    public GameObject ReturnObjectHit()
    {

        if(lastHitObject == null)
        {
            Debug.Log("no object was hit");
            return null;
        }
        return lastHitObject;
    }


    
}
