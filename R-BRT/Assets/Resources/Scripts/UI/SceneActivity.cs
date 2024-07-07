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
    [SerializeField] private GameObject dialogueTriggerTwo;
    [SerializeField] private GameObject dialogueTriggerThree;
    [SerializeField] private GameObject enemyNumberOne;
    [SerializeField] private GameObject dialogueTwoHitBox;
    [SerializeField] private GameObject dialogueThreeHitBox;
    [SerializeField] private GameObject hallwayViewPoint;
    [SerializeField] private GameObject elevatorViewPoint;
    [SerializeField] private GameObject janitorClosetViewPoint;

    [Header("Transforms")]
    [SerializeField] private Transform friendLocation;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform enemyOneTransform;
    [SerializeField] private Transform hallwayTransform;
    [SerializeField] private Transform elevatorTransform;
    [SerializeField] private Transform janitorClosetTransform;

    [Header("Scripts")]
    [SerializeField] private DetectionMeter detectionMeter;
    [SerializeField] private PlayerController playerController;

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

        InitializeViewPoints();
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
        InitializeViewPoints();
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

    private void LoadLevel()
    {
        InitializePlayerAndDetectionMeter();
        Debug.Log("LoadLevel");

        mainCamera = GameObject.FindWithTag("MainCamera")?.transform;
        friendLocation = GameObject.Find("S-4MTired")?.transform;
        playerIsSpotted = false;

        TurnOffTextBoxes(); // Turn off all text boxes when loading the level
    }

    private void TurnOffTextBoxes()
    {
        if (dialogueTriggerOne != null) dialogueTriggerOne.SetActive(false);
        if (dialogueTriggerTwo != null) dialogueTriggerTwo.SetActive(false);
        if (dialogueTriggerThree != null) dialogueTriggerThree.SetActive(false);
        if (dialogueTwoHitBox != null) dialogueTwoHitBox.SetActive(false);
        if (dialogueThreeHitBox != null) dialogueThreeHitBox.SetActive(false);
    }

    private void InitializeTextBoxes()
    {
        dialogueTriggerOne = GameObject.Find("DialoguePanel");
        dialogueTriggerTwo = GameObject.Find("SecondDialogueTrigger");
        dialogueTriggerThree = GameObject.Find("ThirdDialogueTrigger");
        dialogueTwoHitBox = GameObject.Find("SecondDialogueEncounter");
        dialogueThreeHitBox = GameObject.Find("ThirdDialogueEncounter");

        if (dialogueTriggerOne != null) dialogueTriggerOne.SetActive(true);
        if (dialogueTriggerTwo != null) dialogueTriggerTwo.SetActive(false);
        if (dialogueTriggerThree != null) dialogueTriggerThree.SetActive(false);
        if (dialogueTwoHitBox != null) dialogueTwoHitBox.SetActive(true);
        if (dialogueThreeHitBox != null) dialogueThreeHitBox.SetActive(true);
    }

    private void InitializeViewPoints()
    {
        hallwayViewPoint = GameObject.Find("HallwayViewPoint");
        elevatorViewPoint = GameObject.Find("ElevatorViewPoint");
        janitorClosetViewPoint = GameObject.Find("JanitorViewPoint");

        hallwayTransform = hallwayViewPoint != null ? hallwayViewPoint.transform : null;
        elevatorTransform = elevatorViewPoint != null ? elevatorViewPoint.transform : null;
        janitorClosetTransform = janitorClosetViewPoint != null ? janitorClosetViewPoint.transform : null;
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
        enemyOneTransform = enemyNumberOne?.transform;
        if (mainCamera != null && enemyOneTransform != null)
        {
            StartCoroutine(SmoothCameraRotation(mainCamera, enemyOneTransform.position, 2));
        }
        if (playerController != null)
        {
            playerController.SetPlayerActivity(false);
            playerController.SetCameraLock(true);
        }
        if (dialogueTriggerTwo != null) dialogueTriggerTwo.SetActive(true);
        if (dialogueTwoHitBox != null) dialogueTwoHitBox.SetActive(false);
    }

    public void StartThirdDialogue()
    {
        StartCoroutine(StartThirdDialogueSequence());
    }

    private IEnumerator StartThirdDialogueSequence()
    {
        if (playerController != null)
        {
            playerController.SetPlayerActivity(false);
            playerController.SetCameraLock(true);
        }
        if (dialogueThreeHitBox != null) dialogueThreeHitBox.SetActive(false);
        if (dialogueTriggerThree != null) dialogueTriggerThree.SetActive(true);

        // Smooth camera rotation to hallwayTransform
        if (mainCamera != null && hallwayTransform != null)
        {
            yield return SmoothCameraRotation(mainCamera, hallwayTransform.position, 2);
        }
        yield return new WaitForSeconds(2.0f);  // Wait for 2 seconds

        // Smooth camera rotation to elevatorTransform with inverse direction
        if (mainCamera != null && elevatorTransform != null)
        {
            yield return SmoothCameraRotation(mainCamera, elevatorTransform.position, 3, true);
        }
        yield return new WaitForSeconds(2.0f);  // Wait for 2 seconds

        // Smooth camera rotation to janitorClosetTransform
        if (mainCamera != null && janitorClosetTransform != null)
        {
            yield return SmoothCameraRotation(mainCamera, janitorClosetTransform.position, 2);
        }
        yield return new WaitForSeconds(2.0f);  // Wait for 2 seconds

        if (playerController != null)
        {
            playerController.SetPlayerActivity(true);
            playerController.SetCameraLock(false);
        }
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

    public void SetDialogueTriggerTwo(bool value)
    {
        if (dialogueTriggerTwo != null) dialogueTriggerTwo.SetActive(value);
    }

    public void SetDialogueTriggerThree(bool value)
    {
        if (dialogueTriggerThree != null) dialogueTriggerThree.SetActive(value);
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
