using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] GameObject destinationA;
   


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

        else if (other.gameObject.tag == "FourthDialogueEncounter")
        {
            sceneActivity.StartFourthDialogue();
        }

        if (other.gameObject.tag == "FifthDialogueEncounter")
        {
            sceneActivity.StartFifthDialogue();
        }
        
        else if (other.gameObject.tag == "SixthDialogueEncounter")
        {
            
            sceneActivity.StartSixthDialogue();
        }
        if (other.gameObject.tag == "SeventhDialogueEncounter")
        {
            GameManager.instance.SetIndexForAbilityChoice(3);
            sceneActivity.StartStealthDialogue();
            sceneActivity.TurnOffSeventhDialogueBox();
        }
        if (other.gameObject.tag == "EighthDialogueEncounter")
        {
            GameManager.instance.SetIndexForAbilityChoice(2);
            sceneActivity.StartJetPackDialogue();
            sceneActivity.TurnOffEighthDialogueBox();
        }
        else if (other.gameObject.tag == "NinthDialogueEncounter")
        {
            GameManager.instance.SetIndexForAbilityChoice(4);
            sceneActivity.StartJetPackDialogue();
            sceneActivity.TurnOffNinthDialogueBox();
        }
        if (other.gameObject.tag == "TenDialogueEncounter")
        {
            GameManager.instance.SetIndexForAbilityChoice(5);
            sceneActivity.StartStealthDialogue();
            sceneActivity.TurnOffTenthDialogueBox();
        }
        else if(other.gameObject.tag == "EleventhDialogueEncounter")
        {
            sceneActivity.StartEleventhDialogue();
        }
        if (other.gameObject.tag == "TwelthDialogueEncounter")
        {
            sceneActivity.StartTwelthDialogue();
        }

    }
}
