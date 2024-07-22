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
    [SerializeField] private PlayerController playerController;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text interactableText;

    [Header("Animators")]
    [SerializeField] private Animator interactBoxAnim;
    [SerializeField] private Animator interactEAnim;


    [Header("AudioSource")]
    [SerializeField] private AudioSource lorePickUpSound;

    private string[] loreEntryTags = { "LoreEntry", "LoreEntry2", "LoreEntry3", "LoreEntry4" , "LoreEntry5", "LoreEntry6" };

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

                        lorePickUpSound.Play();
                        playerController.FreezePlayer();
                        GameManager.instance.SetLoreEntryPickUp(0,true);
                        GameManager.instance.StartBlinking();
                        GameManager.instance.SetLoreEntryOne(true);
                        GameManager.instance.DisableGameObject(GameManager.instance.ReturnLoreEntryGameObject(0));
                        pauseMenuScript.PauseGame();
                        pauseMenuScript.SwitchToEntriesPanel();
                        break;

                    case "LoreEntry2":

                        lorePickUpSound.Play();
                        playerController.FreezePlayer();
                        GameManager.instance.SetLoreEntryPickUp(1, true);
                        GameManager.instance.StartBlinking();
                        GameManager.instance.SetLoreEntryTwo(true);
                        GameManager.instance.DisableGameObject(GameManager.instance.ReturnLoreEntryGameObject(1));
                        pauseMenuScript.PauseGame();
                        pauseMenuScript.SwitchToEntriesPanel();
                        break;

                    case "LoreEntry3":

                        lorePickUpSound.Play();
                        playerController.FreezePlayer();
                        GameManager.instance.SetLoreEntryPickUp(2, true);
                        GameManager.instance.StartBlinking();
                        GameManager.instance.SetLoreEntryThree(true);
                        GameManager.instance.DisableGameObject(GameManager.instance.ReturnLoreEntryGameObject(2));
                        pauseMenuScript.PauseGame();
                        pauseMenuScript.SwitchToEntriesPanel();
                        break;

                    case "LoreEntry4":

                        lorePickUpSound.Play();
                        playerController.FreezePlayer();
                        GameManager.instance.SetLoreEntryPickUp(2, true);
                        GameManager.instance.StartBlinking();
                        GameManager.instance.SetLoreEntryThree(true);
                        GameManager.instance.DisableGameObject(GameManager.instance.ReturnLoreEntryGameObject(3));
                        pauseMenuScript.PauseGame();
                        pauseMenuScript.SwitchToEntriesPanel();
                        break;

                    case "LoreEntry5":

                        lorePickUpSound.Play();
                        playerController.FreezePlayer();
                        GameManager.instance.SetLoreEntryPickUp(3, true);
                        GameManager.instance.StartBlinking();
                        GameManager.instance.SetLoreEntryFour(true);
                        GameManager.instance.DisableGameObject(GameManager.instance.ReturnLoreEntryGameObject(4));
                        pauseMenuScript.PauseGame();
                        pauseMenuScript.SwitchToEntriesPanel();
                        break;

                    case "LoreEntry6":

                        lorePickUpSound.Play();
                        playerController.FreezePlayer();
                        GameManager.instance.SetLoreEntryPickUp(3, true);
                        GameManager.instance.StartBlinking();
                        GameManager.instance.SetLoreEntryFour(true);
                        GameManager.instance.DisableGameObject(GameManager.instance.ReturnLoreEntryGameObject(5));
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
        playerController = GameManager.instance.ReturnPlayerController();
        lorePickUpSound = GameManager.instance.ReturnLorePickUp();
        interactBoxAnim = GameManager.instance.ReturnInteractBoxAnim();
        interactEAnim = GameManager.instance.ReturnInteractEAnim();
        phoenixChipDecision = GameManager.instance.ReturnPhoenixChipDecision();
        battery = GameManager.instance.ReturnBatteryScript();
        pauseMenuScript = GameManager.instance.ReturnPauseMenu();
    }
}
