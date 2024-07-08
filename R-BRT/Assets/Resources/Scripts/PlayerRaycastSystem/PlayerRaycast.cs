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

    private string[] loreEntryTags = { "LoreEntry", "LoreEntry2", "LoreEntry3", "LoreEntry4" };

    void Start()
    {
        Setup();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, raycastDistance))
        {
            string objectHitTag = hitInfo.collider.gameObject.tag;

            Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red);

            if (hitInfo.distance < interactDistance)
            {
                HandleInteraction(objectHitTag, hitInfo);
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

    void HandleInteraction(string tag, RaycastHit hitInfo)
    {
        if (tag == "PickUpItem")
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
        else if (tag == "PhoenixChip")
        {
            interactableText.text = "Press (E) to pick up the Phoenix Chip";
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactableText.text = "";
                phoenixChipDecision.PlayerDecision();
            }
        }
        else if (tag == "Battery")
        {
            interactableText.text = "Press (E) to pick up the Battery";
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactableText.text = "";
                battery.OpenAbilitiesSelection();
            }
        }
        else if (System.Array.Exists(loreEntryTags, element => element == tag))
        {
            interactableText.text = "Press (E) to pick up the Tablet";
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactableText.text = "";

                switch (tag)
                {
                    case "LoreEntry":

                        GameManager.instance.SetLoreEntryOne(true);
                        GameManager.instance.DestroyGameObject(GameManager.instance.ReturnLoreEntryOneGameObject());
                        break;

                    case "LoreEntry2":

                        GameManager.instance.SetLoreEntryTwo(true);
                        break;

                    case "LoreEntry3":

                        GameManager.instance.SetLoreEntryThree(true);
                        break;

                    case "LoreEntry4":

                        GameManager.instance.SetLoreEntryFour(true);
                        break;

                }
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
