using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{

    [Header("Game Objects")]
    [SerializeField] private GameObject lastHitObject;
    [SerializeField] private GameObject heldObject;
    [SerializeField] private GameObject heldPosition;

    [Header("Floats")]
    [SerializeField] private float interactDistance;
    [SerializeField] private float raycastDistance;
    [SerializeField] private float pickUpCooldown;
    [SerializeField] private float pickUpTime;

    [Header("Bool")]
    [SerializeField] private bool holding;

    [Header("Transform")]
    [SerializeField] private Transform holdingPosition;

    [Header("Scripts")]
    [SerializeField] private PickUpObject pickUpObject;
    [SerializeField] private UIController uI;
    [SerializeField] private Battery battery;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text interactableText;

    void Start()
    {

        interactDistance = 4;
        raycastDistance = interactDistance;
        pickUpCooldown = 0.5f;
        holding = false;
        interactableText = GameObject.FindWithTag("InteractableUIText").GetComponent<TMP_Text>();
        pickUpObject = GameObject.Find("Rocks").GetComponent<PickUpObject>();
        holdingPosition = GameObject.Find("HoldPosition").GetComponent<Transform>();
        heldPosition = GameObject.Find("HoldPosition");
        uI = GameObject.Find("Canvas").GetComponent<UIController>();
        battery = GameObject.FindWithTag("Battery").GetComponent<Battery>();


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
                if (hitInfo.collider.tag == "PickUpItem")
                {
                    interactableText.text = "Press (E) to pick up the rock";

                    if (Input.GetKeyDown(KeyCode.E) && !holding)
                    {
                        interactableText.text = "";
                        holding = true;
                        hitInfo.collider.gameObject.GetComponent<PickUpObject>().PickUp();
                        heldObject = hitInfo.collider.gameObject;
                        pickUpTime = 0f;
                    }
                }

                else if (hitInfo.collider.tag == "PhoenixChip")
                {
                    interactableText.text = "Press (E) to pick up the Phoenix Chip";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactableText.text = "";
                        uI.PhoenixChipDecision();
                    }

                }

                if (hitInfo.collider.tag == "Battery")
                {
                    interactableText.text = "Press (E) to pick up the Battery";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactableText.text = "";
                        battery.OpenAbilitiesSelection();
                    }

                }
            }

            else
            {
                interactableText.text = " ";
            }

            pickUpTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.E) && holding && pickUpTime >= pickUpCooldown)
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

    public void SetInteractableText(string value)
    {
        interactableText.text = value;
    }


    
}
