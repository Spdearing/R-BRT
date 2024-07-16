using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneActivity : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] private bool hasJetPack;
    [SerializeField] private bool hasStealth;
    [SerializeField] private bool playerIsSpotted;
    [SerializeField] private bool firstPlaythrough;
    [SerializeField] private bool firstDialogueHit;
    [SerializeField] private bool secondDialogueHit;
    [SerializeField] private bool thirdDialogueHit;

    [Header("Game Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dialogueTriggerOne;
    [SerializeField] private GameObject abilityDialogueTrigger;
    [SerializeField] private GameObject enemyNumberOne;
    [SerializeField] private GameObject dialogueTwoHitBox;
    [SerializeField] private GameObject dialogueThreeHitBox;
    [SerializeField] private GameObject dialogueFourHitBox;
    [SerializeField] private GameObject dialogueFiveHitBox;
    [SerializeField] private GameObject dialogueSixHitBox;
    [SerializeField] private GameObject dialogueSevenHitBox;
    [SerializeField] private GameObject dialogueEightHitBox;
    [SerializeField] private GameObject dialogueNineHitBox;
    [SerializeField] private GameObject dialogueTenHitBox;
    [SerializeField] private GameObject dialogueElevenHitBox;
    [SerializeField] private GameObject dialogueTwelveHitBox;

    [Header("Transforms")]
    [SerializeField] private Transform friendLocation;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform playerEndRotation;
    

    [Header("Scripts")]
    [SerializeField] private DetectionMeter detectionMeter;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private FirstDialogueFunctionality firstDialogueFunctionality;

    void Start()
    {
        if (GameManager.instance.ReturnNewGameStatus() == true)
        {
            InitializeGameData();
        }
        else

            LoadGame();
       
    }

    private void InitializeGameData()
    {
        firstPlaythrough = true;
        firstDialogueHit = false;
        secondDialogueHit = false;
        thirdDialogueHit = false;

        hasJetPack = false;
        hasStealth = false;
        playerIsSpotted = false;

        InitializePlayerAndDetectionMeter();
        HandleGameSceneLoad();
        player.transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles.x, -309.85f, player.transform.rotation.eulerAngles.z);

    }

    public void LoadGame()
    {
        InitializePlayerAndDetectionMeter();
        InitializeLoadedTextBoxes();
        playerController.SetPlayerActivity(true);
        playerIsSpotted = false;
        if(GameManager.instance.ReturnInvisibilityStatus() == true)
        {
            GameManager.instance.PlayerHasInvisibility();
            Debug.Log("player has Invisibility");
        }
        else if (GameManager.instance.ReturnJetpackStatus() == true)
        {
            GameManager.instance.PlayerHasJetpack();
            Debug.Log("player has Jetpack");
        }
    }

    public void HandleGameSceneLoad()
    {
        if (firstPlaythrough)
        {
            InitializeGameScene();
            if (playerController != null)
            {
                playerController.SetPlayerActivity(false);
            }
        }
    }

    private void InitializeGameScene()
    {
        InitializeTextBoxes();
        InitalizeScripts();
        InitializePlayerAndDetectionMeter();

        mainCamera = GameManager.instance.ReturnCameraTransform();
        friendLocation = GameManager.instance.ReturnFriendsLocation();
        playerIsSpotted = false;

        if (firstPlaythrough)
        {
            if (mainCamera != null && friendLocation != null)
            {
                StartCoroutine(SmoothCameraRotation(mainCamera, player.transform, friendLocation.position, 2));
                if (playerController != null) playerController.SetCameraLock(true);
            }
        }
    }

    private void InitalizeScripts()
    {
        firstDialogueFunctionality =  dialogueTriggerOne.GetComponentInChildren<FirstDialogueFunctionality>();
    }

    private void InitializeTextBoxes()
    {
        dialogueTriggerOne = GameManager.instance.ReturnDialogue();
        abilityDialogueTrigger = GameManager.instance.ReturnAbilityDialogue();
        dialogueTwoHitBox = GameManager.instance.ReturnDialogueTwoHitBox();
        dialogueThreeHitBox = GameManager.instance.ReturnDialogueThreeHitBox();
        dialogueFourHitBox = GameManager.instance.ReturnDialogueFourHitBox();
        dialogueFiveHitBox = GameManager.instance.ReturnDialogueFiveHitBox();
        dialogueSixHitBox = GameManager.instance.ReturnDialogueSixHitBox();
        dialogueSevenHitBox = GameManager.instance.ReturnDialogueSevenHitBox();
        dialogueEightHitBox = GameManager.instance.ReturnDialogueEightHitBox();
        dialogueNineHitBox = GameManager.instance.ReturnDialogueNineHitBox();
        dialogueTenHitBox = GameManager.instance.ReturnDialogueTenHitBox();
        dialogueElevenHitBox = GameManager.instance.ReturnDialogueElevenHitBox();
        dialogueTwelveHitBox = GameManager.instance.ReturnDialogueTwelthHitBox();

        if (dialogueTriggerOne != null) dialogueTriggerOne.SetActive(true);
        if (abilityDialogueTrigger != null) abilityDialogueTrigger.SetActive(false);
        if (dialogueTwoHitBox != null) dialogueTwoHitBox.SetActive(true);
        if (dialogueThreeHitBox != null) dialogueThreeHitBox.SetActive(true);
        if (dialogueFourHitBox != null) dialogueFourHitBox.SetActive(true);
        if (dialogueFiveHitBox != null) dialogueFiveHitBox.SetActive(true);
        if (dialogueSixHitBox != null) dialogueSixHitBox.SetActive(true);
        if (dialogueSevenHitBox != null) dialogueSevenHitBox.SetActive(true);
        if (dialogueEightHitBox != null) dialogueEightHitBox.SetActive(true);
        if (dialogueNineHitBox != null) dialogueNineHitBox.SetActive(true);
        if (dialogueTenHitBox != null) dialogueTenHitBox.SetActive(true);
        if (dialogueElevenHitBox != null) dialogueElevenHitBox.SetActive(true);
        if (dialogueTwelveHitBox != null) dialogueTwelveHitBox.SetActive(true);
    }

    private void InitializeLoadedTextBoxes()
    {
        dialogueTriggerOne = GameManager.instance.ReturnDialogue();
        abilityDialogueTrigger = GameManager.instance.ReturnAbilityDialogue();
        dialogueTwoHitBox = GameManager.instance.ReturnDialogueTwoHitBox();
        dialogueThreeHitBox = GameManager.instance.ReturnDialogueThreeHitBox();
        dialogueFourHitBox = GameManager.instance.ReturnDialogueFourHitBox();
        dialogueFiveHitBox = GameManager.instance.ReturnDialogueFiveHitBox();
        dialogueSixHitBox = GameManager.instance.ReturnDialogueSixHitBox();
        dialogueSevenHitBox = GameManager.instance.ReturnDialogueSevenHitBox();
        dialogueEightHitBox = GameManager.instance.ReturnDialogueEightHitBox();
        dialogueNineHitBox = GameManager.instance.ReturnDialogueNineHitBox();
        dialogueTenHitBox = GameManager.instance.ReturnDialogueTenHitBox();
        dialogueElevenHitBox = GameManager.instance.ReturnDialogueElevenHitBox();
        dialogueTwelveHitBox = GameManager.instance.ReturnDialogueTwelthHitBox();

        if (dialogueTriggerOne != null) dialogueTriggerOne.SetActive(false);
        if (abilityDialogueTrigger != null) abilityDialogueTrigger.SetActive(false);
        if (dialogueTwoHitBox != null) dialogueTwoHitBox.SetActive(true);
        if (dialogueThreeHitBox != null) dialogueThreeHitBox.SetActive(true);
        if (dialogueFourHitBox != null) dialogueFourHitBox.SetActive(true);
        if (dialogueFiveHitBox != null) dialogueFiveHitBox.SetActive(true);
        if (dialogueSixHitBox != null) dialogueSixHitBox.SetActive(true);
        if (dialogueSevenHitBox != null) dialogueSevenHitBox.SetActive(true);
        if (dialogueEightHitBox != null) dialogueEightHitBox.SetActive(true);
        if (dialogueNineHitBox != null) dialogueNineHitBox.SetActive(true);
        if (dialogueTenHitBox != null) dialogueTenHitBox.SetActive(true);
        if (dialogueElevenHitBox != null) dialogueElevenHitBox.SetActive(true);
        if (dialogueTwelveHitBox != null) dialogueTwelveHitBox.SetActive(true);
    }

    private void InitializePlayerAndDetectionMeter()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            player.TryGetComponent(out playerController);
        }

        detectionMeter = GameObject.Find("EnemyDetectionManager")?.GetComponent<DetectionMeter>();
    }

    private IEnumerator SmoothCameraRotation(Transform cameraTransform, Transform playerTransform, Vector3 targetPosition, float duration, bool inverse = false)
    {
        if (cameraTransform == null || playerTransform == null) yield break;

        Quaternion startCameraRotation = cameraTransform.rotation;
        Quaternion startPlayerRotation = playerTransform.rotation;

        Vector3 direction = targetPosition - cameraTransform.position;
        if (inverse)
        {
            direction = -direction;
        }

        Quaternion endRotation = Quaternion.LookRotation(direction);
        Quaternion endCameraRotation = Quaternion.Euler(cameraTransform.eulerAngles.x, endRotation.eulerAngles.y, cameraTransform.eulerAngles.z);
        Quaternion endPlayerRotation = Quaternion.Euler(playerTransform.eulerAngles.x, endRotation.eulerAngles.y, playerTransform.eulerAngles.z);

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cameraTransform.rotation = Quaternion.Lerp(startCameraRotation, endCameraRotation, elapsedTime / duration);
            playerTransform.rotation = Quaternion.Lerp(startPlayerRotation, endPlayerRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraTransform.rotation = endCameraRotation;
        playerTransform.rotation = endPlayerRotation;
    }

    #region//Starting Dialogue
    public void StartSecondDialogue()
    {
        if (dialogueTriggerOne != null)
        {
            dialogueTriggerOne.SetActive(true);
        }
        if (dialogueTwoHitBox != null)
        {
            dialogueTwoHitBox.SetActive(false);
        }     
    }

    public void StartThirdDialogue()
    {
        if (dialogueTriggerOne != null)
        {
            dialogueTriggerOne.SetActive(true);
        }
        if (dialogueThreeHitBox != null)
        {
            dialogueThreeHitBox.SetActive(false);
        }
    }

    public void StartFourthDialogue()
    {
        if (dialogueTriggerOne != null)
        {
            dialogueTriggerOne.SetActive(true);
        }
        if (dialogueFourHitBox != null)
        {
            dialogueFourHitBox.SetActive(false);
        }
    }

    public void StartFifthDialogue()
    {
        if (dialogueTriggerOne != null)
        {
            dialogueTriggerOne.SetActive(true);
        }
        if (dialogueFiveHitBox != null)
        {
            dialogueFiveHitBox.SetActive(false);
        }
    }

    public void StartSixthDialogue()
    {
        if (dialogueTriggerOne != null)
        {
            dialogueTriggerOne.SetActive(true);
        }
        if (dialogueSixHitBox != null)
        {
            dialogueSixHitBox.SetActive(false);
        }
    }

    public void StartTwelthDialogue()
    {
        if (dialogueTriggerOne != null)
        {
            dialogueTriggerOne.SetActive(true);
        }
        if (dialogueTwelveHitBox != null)
        {
            dialogueTwelveHitBox.SetActive(false);
        }
    }


    public void StartEleventhDialogue()
    {
        if (dialogueTriggerOne != null)
        {
            dialogueTriggerOne.SetActive(true);
        }
        if (dialogueElevenHitBox != null)
        {
            dialogueElevenHitBox.SetActive(false);
        }
    }

    public void StartStealthDialogue()
    {
        if (dialogueTriggerOne.activeSelf)
        {
            dialogueTriggerOne.SetActive(false);

            if (abilityDialogueTrigger != null)
            {
                abilityDialogueTrigger.SetActive(true);
            }
        }
        else

            abilityDialogueTrigger.SetActive(true);


    }


    public void StartJetPackDialogue()
    {
        if(dialogueTriggerOne.activeSelf)
        {
            dialogueTriggerOne.SetActive(false);
            
            if (abilityDialogueTrigger != null)
            {
                abilityDialogueTrigger.SetActive(true);
            }
        }
        else
        
            abilityDialogueTrigger.SetActive(true);
        

    }

    public void TurnOffNotJetPackPathDialogue()
    {
        TurnOffSeventhDialogueBox();
        TurnOffTenthDialogueBox();
    }

    public void TurnOffNotStealthPathDialogue()
    {
        TurnOffEighthDialogueBox();
        TurnOffNinthDialogueBox();
    }
    #endregion


    public FirstDialogueFunctionality ReturnFirstDialogueFunctionality()
    {
        return this.firstDialogueFunctionality;
    }


    public void SetDialogueThreeHit(bool value)
    {
        thirdDialogueHit = value;
    }

    public void SetDialogueTwoHit(bool value)
    {
        secondDialogueHit = value;
    }

    public void TurnOffSecondDialogueHitBox()
    {
        if (dialogueTwoHitBox != null) dialogueTwoHitBox.SetActive(false);
    }

    public void TurnOffThirdDialogueHitBox()
    {
        if (dialogueThreeHitBox != null) dialogueThreeHitBox.SetActive(false);
    }

    public void TurnOffSeventhDialogueBox()
    {
        if (dialogueSevenHitBox != null) dialogueSevenHitBox.SetActive(false);
    }

    public void TurnOffEighthDialogueBox()
    {
        if (dialogueEightHitBox != null) dialogueEightHitBox.SetActive(false);
    }

    public void TurnOffNinthDialogueBox()
    {
        if (dialogueNineHitBox != null) dialogueNineHitBox.SetActive(false);
    }

    public void TurnOffTenthDialogueBox()
    {
        if (dialogueTenHitBox != null) dialogueTenHitBox.SetActive(false);
    }

    public void SetEnemyOne(GameObject gameObject)
    {
        this.enemyNumberOne = gameObject;
    }

    public void SetFirstDialogueHit(bool value)
    {
        firstDialogueHit = value;
    }

    public void SetDialogueTriggerOne(bool value)
    {
        if (dialogueTriggerOne != null) dialogueTriggerOne.SetActive(value);
    }

    public void SetDialogueAbilityTrigger(bool value)
    {
        if (abilityDialogueTrigger != null) abilityDialogueTrigger.SetActive(value);
    }

    public void SetJetPackStatus(bool value)
    {
        hasJetPack = value;
    }

    public bool CanUseJetPack()
    {
        return hasJetPack;
    }

    public void SetStealthStatus(bool value)
    {
        hasStealth = value;
    }

    public bool CanUseStealth()
    {
        return hasStealth;
    }

    public GameObject ReturnPlayer()
    {
        return player;
    }

    public DetectionMeter ReturnDetectionMeter()
    {
        return detectionMeter;
    }

    public bool ReturnPlayerSpotted()
    {
        return playerIsSpotted;
    }

    public void SetPlayerIsSpotted(bool value)
    {
        playerIsSpotted = value;
    }
}
