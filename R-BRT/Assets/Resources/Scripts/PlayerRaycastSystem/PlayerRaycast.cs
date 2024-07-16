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
    [SerializeField] private PauseMenu pauseMenuScript;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text interactableText;

    [Header("Animators")]
    [SerializeField] private Animator interactBoxAnim;
    [SerializeField] private Animator interactEAnim;

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
            

            pickUpTime += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.E) && holding && pickUpTime >= pickUpCooldown)
            {
                holding = false;
                heldObject.GetComponent<PickUpObject>().PutDown();
                heldObject = null;
            }
        }
        else
        {
            interactBoxAnim.SetBool("Interact", false);
            interactEAnim.SetBool("Interact", false);
        }
    }

    void HandleInteraction(string tag, RaycastHit hitInfo)
    {
        if (tag == "PickUpItem")
        {
            Debug.Log(tag);
            interactBoxAnim.SetBool("Interact", true);
            interactEAnim.SetBool("Interact", true);


            if (Input.GetKeyDown(KeyCode.E) && !holding)
            {
                
                holding = true;
                hitInfo.collider.gameObject.GetComponent<PickUpObject>().PickUp();
                heldObject = hitInfo.collider.gameObject;
                pickUpTime = 0f;
            }
        }

        
        if (tag == "PhoenixChip")
        {
            interactBoxAnim.SetBool("Interact", true);
            interactEAnim.SetBool("Interact", true);

            if (Input.GetKeyDown(KeyCode.E))
            {
               
                phoenixChipDecision.PlayerDecision();
            }
        }

        

        if (tag == "Battery")
        {
            interactBoxAnim.SetBool("Interact", true);
            interactEAnim.SetBool("Interact", true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                
                battery.OpenAbilitiesSelection();
            }
        }
        
        if (System.Array.Exists(loreEntryTags, element => element == tag))
        {
            interactBoxAnim.SetBool("Interact", true);
            interactEAnim.SetBool("Interact", true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                
                switch (tag)
                {
                    case "LoreEntry":

                        GameManager.instance.SetLoreEntryOne(true);
                        GameManager.instance.DestroyGameObject(GameManager.instance.ReturnLoreEntryOneGameObject());
                        pauseMenuScript.PauseGame();
                        pauseMenuScript.SwitchToEntriesPanel();
                        break;

                    case "LoreEntry2":

                        GameManager.instance.SetLoreEntryTwo(true);
                        GameManager.instance.DestroyGameObject(GameManager.instance.ReturnLoreEntryTwoGameObject());
                        pauseMenuScript.PauseGame();
                        pauseMenuScript.SwitchToEntriesPanel();
                        break;

                    case "LoreEntry3":

                        GameManager.instance.SetLoreEntryThree(true);
                        GameManager.instance.DestroyGameObject(GameManager.instance.ReturnLoreEntryThreeGameObject());
                        pauseMenuScript.PauseGame();
                        pauseMenuScript.SwitchToEntriesPanel();
                        break;

                    case "LoreEntry4":

                        GameManager.instance.SetLoreEntryFour(true);
                        GameManager.instance.DestroyGameObject(GameManager.instance.ReturnLoreEntryFourGameObject());
                        pauseMenuScript.PauseGame();
                        pauseMenuScript.SwitchToEntriesPanel();
                        break;

                }
            }
        }

    }

    void Setup()
    {
        interactDistance = 1.5f;
        raycastDistance = interactDistance;
        pickUpCooldown = 0.5f;
        holding = false;
        interactBoxAnim = GameManager.instance.ReturnInteractBoxAnim();
        interactEAnim = GameManager.instance.ReturnInteractEAnim();
        phoenixChipDecision = GameManager.instance.ReturnPhoenixChipDecision();
        battery = GameManager.instance.ReturnBatteryScript();
        pauseMenuScript = GameManager.instance.ReturnPauseMenu();
    }
}
