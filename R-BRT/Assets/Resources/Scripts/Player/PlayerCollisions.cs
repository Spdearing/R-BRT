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
       if (other.gameObject.tag == "SecondDialogueEncounter")
        {
            firstDialogueFunctionality.SetDialogue("Second Dialogue");
            sceneActivity.StartSecondDialogue();
        }

        else if (other.gameObject.tag == "ThirdDialogueEncounter")
        {
            firstDialogueFunctionality.SetDialogue("Third Dialogue");
            sceneActivity.StartThirdDialogue();
        }

        if (other.gameObject.tag == "FourthDialogueEncounter")
        {
            firstDialogueFunctionality.SetDialogue("Fourth Dialogue");
            sceneActivity.StartFourthDialogue();
        }

        else if (other.gameObject.tag == "FifthDialogueEncounter")
        {
            firstDialogueFunctionality.SetDialogue("Fifth Dialogue");
            sceneActivity.StartFifthDialogue();
        }
        
        if (other.gameObject.tag == "SixthDialogueEncounter")
        {
            firstDialogueFunctionality.SetDialogue("Sixth Dialogue");
            sceneActivity.StartSixthDialogue();
        }
        else if (other.gameObject.tag == "SeventhDialogueEncounter")
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
            firstDialogueFunctionality.SetDialogue("Eighth Dialogue");
            sceneActivity.StartEleventhDialogue();
        }
        if (other.gameObject.tag == "TwelthDialogueEncounter")
        {
            firstDialogueFunctionality.SetDialogue("Seventh Dialogue");
            sceneActivity.StartTwelthDialogue();
        }
        else if(other.gameObject.tag == "After Lobby")
        {
            GameManager.instance.AddSpawnPoint(other.gameObject.transform);
            
        }
        if (other.gameObject.tag == "Janitors Closet")
        {
            GameManager.instance.AddSpawnPoint(other.gameObject.transform);
            
        }
        else if (other.gameObject.tag == "Top Elevator")
        {
            GameManager.instance.AddSpawnPoint(other.gameObject.transform);
            GameManager.instance.SetJetpackStatus(true);
            GameManager.instance.ReturnPlayerCheckPoint(3).SetActive(false);
            GameManager.instance.ReturnPlayerCheckPoint(4).SetActive(false);

        }
        if (other.gameObject.tag == "Top Stairs")
        {
            GameManager.instance.AddSpawnPoint(other.gameObject.transform);
            GameManager.instance.SetInvisibilityStatus(true);
            GameManager.instance.ReturnPlayerCheckPoint(2).SetActive(false);
            GameManager.instance.ReturnPlayerCheckPoint(5).SetActive(false);

        }
        else if (other.gameObject.tag == "Second Broken Room")
        {
            GameManager.instance.AddSpawnPoint(other.gameObject.transform);
           
        }
        if (other.gameObject.tag == "Before Jetpack Puzzle")
        {
            GameManager.instance.AddSpawnPoint(other.gameObject.transform);
            
        }
    }
}
