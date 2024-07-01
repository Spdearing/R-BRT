using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
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

    [Header("GameManager Instance")]
    public static GameManager Instance;

    [Header("Scripts")]
    [SerializeField] private DetectionMeter detectionMeter;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GroundBotSpawner groundBotSpawner;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This makes the GameObject persistent across scenes
            firstPlaythrough = true;
            firstDialogueHit = false;
            secondDialogueHit = false;
            thirdDialogueHit = false;
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

    void Start()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        switch (scene.name)
        {
            case "GameScene":
                Debug.Log("First playthrough");
                HandleGameSceneLoad();
                break;

            case "SamDies":
            case "VictorySamLives":
                Time.timeScale = 1;
                StartCoroutine(TransitionBackToStart());
                break;

            case "Player_Enemy_TestScene":
                Time.timeScale = 1.0f;
                InitializePlayerAndDetectionMeter();
                break;

            default:
                Debug.LogWarning($"Scene '{scene.name}' not handled in OnSceneLoaded");
                break;
        }
    }

    private void HandleGameSceneLoad()
    {
        if (firstPlaythrough && !firstDialogueHit)
        {
            InitializeGameScene();
            playerController.SetPlayerActivity(false);
        }
        else
        {
            LoadLevel();
        }
    }

    private void InitializeGameScene()
    {
        dialogueTriggerOne = GameObject.FindWithTag("DialogueTriggerOne");
        dialogueTriggerTwo = GameObject.FindWithTag("DialogueTriggerTwo");
        dialogueTriggerThree = GameObject.FindWithTag("DialogueTriggerThree");
        dialogueTwoHitBox = GameObject.FindWithTag("SecondDialogueEncounter");
        dialogueThreeHitBox = GameObject.FindWithTag("ThirdDialogueEncounter");
        Debug.Log("turning second text ");
        dialogueTriggerTwo.SetActive(false);
        Debug.Log("turning Off Third Dialogue");
        dialogueTriggerThree.SetActive(false);
        Debug.Log("turning first text on");
        dialogueTriggerOne.SetActive(true);
        Debug.Log("turning hit box on");
        dialogueTwoHitBox.SetActive(true);
        Debug.Log("turning hit box 3 on");
        dialogueThreeHitBox.SetActive(true);

        hallwayViewPoint = GameObject.Find("HallwayViewPoint");
        elevatorViewPoint = GameObject.Find("ElevatorViewPoint");
        janitorClosetViewPoint = GameObject.Find("JanitorViewPoint");
        hallwayTransform = hallwayViewPoint.GetComponent<Transform>();
        elevatorTransform = elevatorViewPoint.GetComponent<Transform>();
        janitorClosetTransform = janitorClosetViewPoint.GetComponent<Transform>();

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        detectionMeter = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        friendLocation = GameObject.Find("S-4MTired").GetComponent<Transform>();
        playerIsSpotted = false;

        if (firstPlaythrough)
        {
            StartCoroutine(SmoothCameraRotationToFriend(mainCamera, friendLocation.position, 2));
            playerController.SetCameraLock(true);
        }
    }

    private void LoadLevel()
    {
        InitializePlayerAndDetectionMeter();
        Debug.Log("LoadLevel");

        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        friendLocation = GameObject.Find("S-4MTired").GetComponent<Transform>();
        playerIsSpotted = false;
    }

    private void InitializePlayerAndDetectionMeter()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        detectionMeter = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>();
    }

    private IEnumerator SmoothCameraRotationToFriend(Transform cameraTransform, Vector3 targetPosition, float duration)
    {
        Quaternion startRotation = cameraTransform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(targetPosition - cameraTransform.position);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cameraTransform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cameraTransform.rotation = endRotation;
    }

    private IEnumerator SmoothCameraRotationToFirstEnemy(Transform cameraTransform, Vector3 targetPosition, float duration)
    {
        Quaternion startRotation = cameraTransform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(targetPosition - cameraTransform.position);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cameraTransform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cameraTransform.rotation = endRotation;
    }

    private IEnumerator SmoothCameraRotationHallway(Transform cameraTransform, Vector3 targetPosition, float duration)
    {
        Quaternion startRotation = cameraTransform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(targetPosition - cameraTransform.position);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            cameraTransform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(2.0f);
        }
        cameraTransform.rotation = endRotation;
    }

    public void StartSecondDialogue()
    {
        enemyOneTransform = enemyNumberOne.transform;
        StartCoroutine(SmoothCameraRotationToFirstEnemy(mainCamera, enemyOneTransform.position, 2));
        playerController.SetPlayerActivity(false);
        playerController.SetCameraLock(true);
        dialogueTriggerTwo.SetActive(true);
        dialogueTwoHitBox.SetActive(false);
    }

    public void StartThirdDialogue()
    {
        StartCoroutine(SmoothCameraRotationHallway(mainCamera, hallwayTransform.position, 2));
        StartCoroutine(SmoothCameraRotationHallway(mainCamera, elevatorTransform.position, 2));
        StartCoroutine(SmoothCameraRotationHallway(mainCamera, janitorClosetTransform.position, 2));
        playerController.SetPlayerActivity(false);
        playerController.SetCameraLock(true);
        dialogueTriggerThree.SetActive(true);
        dialogueThreeHitBox.SetActive(false);
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
        dialogueTwoHitBox.SetActive(false);
    }

    public void TurnOffThirdDialogueHitBox()
    {
        dialogueThreeHitBox.SetActive(false);
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
        dialogueTriggerOne.SetActive(value);
    }

    public void SetDialogueTriggerTwo(bool value)
    {
        dialogueTriggerTwo.SetActive(value);
    }

    public void SetDialogueTriggerThree(bool value)
    {
        dialogueTriggerThree.SetActive(value);
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

    private IEnumerator TransitionBackToStart()
    {
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene("MainMenuScene");
    }
}