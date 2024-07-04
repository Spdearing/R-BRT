using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{

    [Header("Game Objects")]
    [SerializeField] private GameObject lastHitObject;
    [SerializeField] private GameObject heldObject;


    [Header("Floats")]
    [SerializeField] private float interactDistance;
    [SerializeField] private float raycastDistance;
    [SerializeField] private float pickUpCooldown;
    [SerializeField] private float pickUpTime;

    [Header("Bool")]
    [SerializeField] private bool holding;

    [Header("Scripts")]
    [SerializeField] private PhoenixChipDecision phoenixChipDecision;
    [SerializeField] private Battery battery;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text interactableText;

    void Start()
    {
        Setup();
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
                        phoenixChipDecision.PlayerDecision();
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

    void Setup()
    {
        interactDistance = 4;
        raycastDistance = interactDistance;
        pickUpCooldown = 0.5f;
        holding = false;
        interactableText = GameManager.instance.ReturnInteractableText();
        phoenixChipDecision = GameManager.instance.ReturnPhoenixChipDecision();
        battery = GameManager.instance.ReturnBatteryScript();
    }

    public void SetInteractableText(string value)
    {
        interactableText.text = value;
    }


    
}
