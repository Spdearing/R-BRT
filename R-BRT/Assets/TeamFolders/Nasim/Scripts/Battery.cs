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
        //playerController.TurnJetPackoff();
        
        
    }

    public void OpenAbilitiesSelection()
    {
        Time.timeScale = 0;
        abilitySelectionPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



    public void OnClickJetpackButton()
    {
        //playerController.TurnJetPackOn();
        fuelMeter.SetActive(true);
        abilitySelectionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        Destroy(battery);
    }

    public void OnClickInvisibleButton()
    {
        playerController.SetInvisibilityUnlock(true);
        playerController.DisplayInvisibilityMeter();
        abilitySelectionPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        Destroy(battery);
    }
}
