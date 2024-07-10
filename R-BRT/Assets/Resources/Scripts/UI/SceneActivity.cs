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
    [SerializeField] private GameObject enemyNumberOne;
    [SerializeField] private GameObject dialogueTwoHitBox;
    [SerializeField] private GameObject dialogueThreeHitBox;
    [SerializeField] private GameObject dialogueFourHitBox;
    [SerializeField] private GameObject dialogueFiveHitBox;

    [Header("Transforms")]
    [SerializeField] private Transform friendLocation;
    [SerializeField] private Transform mainCamera;

    [Header("Scripts")]
    [SerializeField] private DetectionMeter detectionMeter;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private FirstDialogueFunctionality firstDialogueFunctionality;

    void Start()
    {
        InitializeGameData();
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
                StartCoroutine(SmoothCameraRotation(mainCamera, friendLocation.position, 2));
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
        dialogueTwoHitBox = GameManager.instance.ReturnDialogueTwoHitBox();
        dialogueThreeHitBox = GameManager.instance.ReturnDialogueThreeHitBox();
        dialogueFourHitBox = GameManager.instance.ReturnDialogueFourHitBox();
        dialogueFiveHitBox = GameManager.instance.ReturnDialogueFiveHitBox();

        if (dialogueTriggerOne != null) dialogueTriggerOne.SetActive(true);
        if (dialogueTwoHitBox != null) dialogueTwoHitBox.SetActive(true);
        if (dialogueThreeHitBox != null) dialogueThreeHitBox.SetActive(true);
        if (dialogueFourHitBox != null) dialogueFourHitBox.SetActive(true);
        if (dialogueFiveHitBox != null) dialogueFiveHitBox.SetActive(true);
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

    private IEnumerator SmoothCameraRotation(Transform cameraTransform, Vector3 targetPosition, float duration, bool inverse = false)
    {
        if (cameraTransform == null) yield break;

        Quaternion startRotation = cameraTransform.rotation;
        Vector3 direction = targetPosition - cameraTransform.position;
        if (inverse)
        {
            direction = -direction;
        }
        Quaternion endRotation = Quaternion.LookRotation(direction);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cameraTransform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cameraTransform.rotation = endRotation;
    }

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
            dialogueTwoHitBox.SetActive(false);
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
            dialogueTwoHitBox.SetActive(false);
        }
    }

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
