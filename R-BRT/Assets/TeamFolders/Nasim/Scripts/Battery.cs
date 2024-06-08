using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    Rigidbody rb;
    public GameObject player;
    Jetpack jetPack;

    public Canvas GUICanvas;
    public GameObject interactableText;

    // Start is called before the first frame update
    void Start()
    {
        GUICanvas.gameObject.SetActive(false);
        interactableText.gameObject.SetActive(false);

        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Battery";
        jetPack = player.GetComponent<Jetpack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            interactableText.gameObject.SetActive(true);
            GUICanvas.gameObject.SetActive(true);
        }
        /*jetPack.enabled = true;
        Destroy(gameObject);*/
    }
}
