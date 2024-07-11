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
    [SerializeField] private GameObject invisibilityMeter;

    [Header("Image")]
    [SerializeField] private Image invisibilityMeterImage;

    [Header("Buttons")]
    [SerializeField] private Button jetpackButton;
    [SerializeField] private Button stealthButton;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerRaycast playerRayCast;
    [SerializeField] private PlayerAbilities abilities;
    [SerializeField] private SceneActivity sceneActivity; 





    // Start is called before the first frame update
    void Start()
    { 
        
        playerRayCast = GameManager.instance.ReturnPlayerRaycast();
        battery = GameManager.instance.ReturnBatteryObject();
        fuelMeter = GameManager.instance.ReturnFuelMeter();
        invisibilityMeter = GameManager.instance.ReturnInvisibilityMeterGameObject();
        abilities = GameManager.instance.ReturnPlayerAbilities();
        sceneActivity = GameManager.instance.ReturnSceneActivity();
        gameObject.tag = "Battery";
        fuelMeter.SetActive(false);
        invisibilityMeter.SetActive(false);
        abilitySelectionPanel.SetActive(false);
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
        GameManager.instance.SetIndexForAbilityChoice(1);
        
        playerController.SetPlayerActivity(true);
        playerController.SetCameraLock(false);
        playerRayCast.SetInteractableText("");
        abilities.SetJetPackUnlock(true);
        fuelMeter.SetActive(true);
        abilitySelectionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sceneActivity.StartJetPackDialogue();
        Destroy(battery);
    }

    public void OnClickInvisibleButton()
    {
        GameManager.instance.SetIndexForAbilityChoice(0);
        playerController.SetPlayerActivity(true);
        playerController.SetCameraLock(false);
        playerRayCast.SetInteractableText("");
        abilities.SetInvisibilityUnlock(true);
        abilities.DisplayInvisibilityMeter();
        abilitySelectionPanel.SetActive(false);
        invisibilityMeter.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sceneActivity.StartStealthDialogue();
        Destroy(battery);
    }
}
