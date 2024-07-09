using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] GameObject destinationA;
    [SerializeField] GameObject destinationB;


    [SerializeField] private SceneActivity sceneActivity;
    [SerializeField] private FirstDialogueFunctionality firstDialogueFunctionality;

    private void Start()
    {
        sceneActivity = GameManager.instance.ReturnSceneActivity();
        firstDialogueFunctionality = GameManager.instance.ReturnFirstDialogueFunctionality();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Death")
        {
            transform.position = destinationA.transform.position;
        }

        else if (other.gameObject.tag == "SecondDialogueEncounter")
        {
            
            sceneActivity.StartSecondDialogue();
        }

        if (other.gameObject.tag == "ThirdDialogueEncounter")
        {
            sceneActivity.StartThirdDialogue();
        }
    }
}
