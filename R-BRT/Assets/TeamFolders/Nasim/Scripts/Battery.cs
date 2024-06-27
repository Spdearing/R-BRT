using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battery : MonoBehaviour
{

    [Header("Game Objects")]
    [SerializeField] private GameObject abilitySelectionPanel;
    [SerializeField] private GameObject battery;
    [SerializeField] private GameObject fuelMeter;

    [Header("Buttons")]
    [SerializeField] private Button jetpackButton;
    [SerializeField] private Button stealthButton;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerRaycast playerRayCast;

    [Header("TMP_Text")]
    [SerializeField] private TMP_Text interactableText;




    // Start is called before the first frame update
    void Start()
    { 
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        battery = GameObject.FindWithTag("Battery");
        fuelMeter = GameObject.FindWithTag("FuelMeter");
        gameObject.tag = "Battery";
        fuelMeter.SetActive(false);
        abilitySelectionPanel.SetActive(false);
        playerRayCast = GameObject.FindWithTag("MainCamera").GetComponent<PlayerRaycast>();
    }

    public void OpenAbilitiesSelection()
    {
        playerController.SetPlayerActivity(false);
        abilitySelectionPanel.SetActive(true);
        playerRayCast.SetInteractableText("");
        playerController.isCameraLocked = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



    public void OnClickJetpackButton()
    {
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        playerRayCast.SetInteractableText("");
        playerController.SetJetPackUnlock(true);
        fuelMeter.SetActive(true);
        abilitySelectionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Destroy(battery);
    }

    public void OnClickInvisibleButton()
    {
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        playerRayCast.SetInteractableText("");
        playerController.SetInvisibilityUnlock(true);
        playerController.DisplayInvisibilityMeter();
        abilitySelectionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Destroy(battery);
    }
}
