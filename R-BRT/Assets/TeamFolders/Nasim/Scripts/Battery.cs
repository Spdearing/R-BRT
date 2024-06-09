using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    Rigidbody rb;
    public GameObject player;
    Jetpack jetPack;
    PlayerController invisibility;

    public Canvas GUICanvas;
    public GameObject interactableText;
    public GameObject jetpackButton;
    public GameObject stealthButton;

    PlayerController playerCharacterController;

    // Start is called before the first frame update
    void Start()
    {
        GUICanvas.gameObject.SetActive(false);

        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Battery";
        jetPack = player.GetComponent<Jetpack>();
        invisibility = player.GetComponent<PlayerController>();
        playerCharacterController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerCharacterController.isCameraLocked = true;
            interactableText.gameObject.SetActive(true);
            GUICanvas.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Destroy(gameObject);
        }
    }

    public void OnClickJetpackButton()
    {
        jetPack.enabled = true;

        playerCharacterController.isCameraLocked = false;
        interactableText.gameObject.SetActive(false);
        GUICanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnClickInvisibleButton()
    {
        invisibility.enabled = true;

        playerCharacterController.isCameraLocked = false;
        interactableText.gameObject.SetActive(false);
        GUICanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
