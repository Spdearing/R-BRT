using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{

    [Header("Game Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject interactableText;
    [SerializeField] private GameObject abilitySelectionPanel;
    [SerializeField] private GameObject jetpackButton;
    [SerializeField] private GameObject stealthButton;
    [SerializeField] private GameObject battery;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Jetpack jetPack;


    // Start is called before the first frame update
    void Start()
    {
        
        gameObject.tag = "Battery";
        abilitySelectionPanel.SetActive(false);
        jetPack = GameObject.Find("Player").GetComponent<Jetpack>();
        
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            abilitySelectionPanel.SetActive(true);
            interactableText.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OnClickJetpackButton()
    {
        
        jetPack.enabled = true;

        
        abilitySelectionPanel.SetActive(false);
        interactableText.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        Destroy(battery);
    }

    public void OnClickInvisibleButton()
    {
        playerController.SetInvisibilityUnlock(true);
        abilitySelectionPanel.SetActive(false);
        interactableText.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        Destroy(battery);
    }
}
