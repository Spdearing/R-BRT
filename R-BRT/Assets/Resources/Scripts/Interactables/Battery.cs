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
    [SerializeField] private GameObject stealthBaracade;
    [SerializeField] private GameObject elevatorBaracade;

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
        abilitySelectionPanel.SetActive(false);
        elevatorBaracade = GameManager.instance.ReturnElevatorBlockade();
        stealthBaracade = GameManager.instance.ReturnStealthBlockade();
    }

    public void OpenAbilitiesSelection()
    {
        abilitySelectionPanel.SetActive(true);
        playerController.SetPlayerActivity(false);
        playerController.SetCameraLock(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnClickJetpackButton()
    {
        GameManager.instance.SetIndexForAbilityChoice(1);
        sceneActivity.TurnOffNotJetPackPathDialogue();
        GameManager.instance.CloseOffTheStairs();
        GameManager.instance.SetHasPickedAbility(true);
        stealthBaracade.SetActive(true);
        playerController.SetPlayerActivity(true);
        playerController.SetCameraLock(false);
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
        sceneActivity.TurnOffNotStealthPathDialogue();
        GameManager.instance.SetHasPickedAbility(true);
        elevatorBaracade.SetActive(true);
        playerController.SetPlayerActivity(true);
        playerController.SetCameraLock(false);
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
