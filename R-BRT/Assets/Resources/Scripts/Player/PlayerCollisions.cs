using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] GameObject destinationA;

    [SerializeField] private SceneActivity sceneActivity;
    [SerializeField] private FirstDialogueFunctionality firstDialogueFunctionality;

    private Dictionary<string, System.Action<Collider>> collisionActions;

    private void Start()
    {
        sceneActivity = GameManager.instance.ReturnSceneActivity();
        firstDialogueFunctionality = GameManager.instance.ReturnFirstDialogueFunctionality();

        InitializeCollisionActions();
    }

    private void InitializeCollisionActions()
    {
        collisionActions = new Dictionary<string, System.Action<Collider>>()
        {
            { "SecondDialogueEncounter", SecondDialogueEncounter },
            { "ThirdDialogueEncounter", ThirdDialogueEncounter },
            { "FourthDialogueEncounter", FourthDialogueEncounter },
            { "FifthDialogueEncounter", FifthDialogueEncounter },
            { "SixthDialogueEncounter", SixthDialogueEncounter },
            { "SeventhDialogueEncounter", SeventhDialogueEncounter },
            { "EighthDialogueEncounter", EighthDialogueEncounter },
            { "NinthDialogueEncounter", NinthDialogueEncounter },
            { "TenDialogueEncounter", TenDialogueEncounter },
            { "EleventhDialogueEncounter", EleventhDialogueEncounter },
            { "TwelthDialogueEncounter", TwelthDialogueEncounter },
            { "BeginingOfLobby", BeginningOfLobby },
            { "After Lobby", AfterLobby },
            { "Janitors Closet", JanitorsCloset },
            { "Top Of Elevator", TopElevator },
            { "Top Stairs", TopStairs },
            { "Second Broken Room", SecondBrokenRoom },
            { "Before Jetpack Puzzle", BeforeJetpackPuzzle },
            { "Close Janitors Closet", CloseJanitorsCloset }  
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collisionActions.TryGetValue(other.gameObject.tag, out System.Action<Collider> action))
        {
            action.Invoke(other);
        }
    }

    private void SecondDialogueEncounter(Collider other)
    {
        firstDialogueFunctionality.SetDialogue("Second Dialogue");
        sceneActivity.StartSecondDialogue();
    }

    private void ThirdDialogueEncounter(Collider other)
    {
        firstDialogueFunctionality.SetDialogue("Third Dialogue");
        sceneActivity.StartThirdDialogue();
    }

    private void FourthDialogueEncounter(Collider other)
    {
        firstDialogueFunctionality.SetDialogue("Fourth Dialogue");
        sceneActivity.StartFourthDialogue();
    }

    private void FifthDialogueEncounter(Collider other)
    {
        firstDialogueFunctionality.SetDialogue("Fifth Dialogue");
        sceneActivity.StartFifthDialogue();
    }

    private void SixthDialogueEncounter(Collider other)
    {
        firstDialogueFunctionality.SetDialogue("Sixth Dialogue");
        sceneActivity.StartSixthDialogue();
    }

    private void SeventhDialogueEncounter(Collider other)
    {
        GameManager.instance.SetIndexForAbilityChoice(3);
        sceneActivity.StartStealthDialogue();
        sceneActivity.TurnOffSeventhDialogueBox();
        sceneActivity.TurnOffNotStealthPathDialogue();
    }

    private void EighthDialogueEncounter(Collider other)
    {
        GameManager.instance.SetIndexForAbilityChoice(2);
        sceneActivity.StartJetPackDialogue();
        sceneActivity.TurnOffEighthDialogueBox();
        sceneActivity.TurnOffNotJetPackPathDialogue();
    }

    private void NinthDialogueEncounter(Collider other)
    {
        GameManager.instance.SetIndexForAbilityChoice(4);
        sceneActivity.StartJetPackDialogue();
        sceneActivity.TurnOffNinthDialogueBox();
        sceneActivity.TurnOffNotJetPackPathDialogue();
    }

    private void TenDialogueEncounter(Collider other)
    {
        GameManager.instance.SetIndexForAbilityChoice(5);
        sceneActivity.StartStealthDialogue();
        sceneActivity.TurnOffTenthDialogueBox();
        sceneActivity.TurnOffNotStealthPathDialogue();
    }

    private void EleventhDialogueEncounter(Collider other)
    {
        firstDialogueFunctionality.SetDialogue("Eighth Dialogue");
        sceneActivity.StartEleventhDialogue();
    }

    private void TwelthDialogueEncounter(Collider other)
    {
        firstDialogueFunctionality.SetDialogue("Seventh Dialogue");
        sceneActivity.StartTwelthDialogue();
    }

    private void BeginningOfLobby(Collider other)
    {
        GameManager.instance.AddSpawnPoint(other.gameObject.transform);
    }

    private void AfterLobby(Collider other)
    {
        GameManager.instance.AddSpawnPoint(other.gameObject.transform);
    }

    private void JanitorsCloset(Collider other)
    {
        GameManager.instance.AddSpawnPoint(other.gameObject.transform);
    }

    private void TopElevator(Collider other)
    {
        GameManager.instance.AddSpawnPoint(other.gameObject.transform);
        GameManager.instance.SetJetpackStatus(true);
        GameManager.instance.ReturnPlayerCheckPoint(3).SetActive(false);
        GameManager.instance.ReturnPlayerCheckPoint(4).SetActive(false);
        GameManager.instance.SetPlayerHasClearedHallway(true);
        GameManager.instance.CloseOffTheStairs();
    }

    private void TopStairs(Collider other)
    {
        GameManager.instance.AddSpawnPoint(other.gameObject.transform);
        GameManager.instance.SetInvisibilityStatus(true);
        GameManager.instance.ReturnPlayerCheckPoint(2).SetActive(false);
        GameManager.instance.ReturnPlayerCheckPoint(5).SetActive(false);
        GameManager.instance.SetPlayerHasClearedHallway(true);
    }

    private void SecondBrokenRoom(Collider other)
    {
        GameManager.instance.AddSpawnPoint(other.gameObject.transform);
    }

    private void BeforeJetpackPuzzle(Collider other)
    {
        GameManager.instance.AddSpawnPoint(other.gameObject.transform);
    }

    private void CloseJanitorsCloset(Collider other)
    {
        if (GameManager.instance.CheckIfPickedUpAbility())
        {
            GameManager.instance.ShutJanitorsCloset();
        }
    }
}
