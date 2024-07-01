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
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

    void OnEnable()
    {
        InitializeGameData();
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

        InitializeTextBoxes();
        InitializeViewPoints();
        InitializePlayerAndDetectionMeter();
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
        InitializeTextBoxes();
        InitializeViewPoints();
        InitializePlayerAndDetectionMeter();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        friendLocation = GameObject.Find("S-4MTired").GetComponent<Transform>();
        playerIsSpotted = false;

        if (firstPlaythrough)
        {
            StartCoroutine(SmoothCameraRotation(mainCamera, friendLocation.position, 2));
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
        dialogueTriggerOne = GameObject.FindWithTag("DialogueTriggerOne");
        dialogueTriggerTwo = GameObject.FindWithTag("DialogueTriggerTwo");
        dialogueTriggerThree = GameObject.FindWithTag("DialogueTriggerThree");
        dialogueTwoHitBox = GameObject.FindWithTag("SecondDialogueEncounter");
        dialogueThreeHitBox = GameObject.FindWithTag("ThirdDialogueEncounter");

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

        hallwayTransform = hallwayViewPoint != null ? hallwayViewPoint.GetComponent<Transform>() : null;
        elevatorTransform = elevatorViewPoint != null ? elevatorViewPoint.GetComponent<Transform>() : null;
        janitorClosetTransform = janitorClosetViewPoint != null ? janitorClosetViewPoint.GetComponent<Transform>() : null;
    }

    private void InitializePlayerAndDetectionMeter()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

        detectionMeter = GameObject.Find("EnemyDetectionManager")?.GetComponent<DetectionMeter>();
    }

    private IEnumerator SmoothCameraRotation(Transform cameraTransform, Vector3 targetPosition, float duration, bool inverse = false)
    {
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
        enemyOneTransform = enemyNumberOne.transform;
        StartCoroutine(SmoothCameraRotation(mainCamera, enemyOneTransform.position, 2));
        playerController.SetPlayerActivity(false);
        playerController.SetCameraLock(true);
        dialogueTriggerTwo.SetActive(true);
        dialogueTwoHitBox.SetActive(false);
    }

    public void StartThirdDialogue()
    {
        StartCoroutine(StartThirdDialogueSequence());
    }

    private IEnumerator StartThirdDialogueSequence()
    {
        playerController.SetPlayerActivity(false);
        dialogueThreeHitBox.SetActive(false);
        playerController.SetCameraLock(true);
        dialogueTriggerThree.SetActive(true);

        // Smooth camera rotation to hallwayTransform
        yield return SmoothCameraRotation(mainCamera, hallwayTransform.position, 2);
        yield return new WaitForSeconds(2.0f);  // Wait for 2 seconds

        // Smooth camera rotation to elevatorTransform with inverse direction
        yield return SmoothCameraRotation(mainCamera, elevatorTransform.position, 3, true);
        yield return new WaitForSeconds(2.0f);  // Wait for 2 seconds

        // Smooth camera rotation to janitorClosetTransform
        yield return SmoothCameraRotation(mainCamera, janitorClosetTransform.position, 2);
        yield return new WaitForSeconds(2.0f);  // Wait for 2 seconds

        playerController.SetPlayerActivity(true);
        playerController.SetCameraLock(false);
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

    private IEnumerator TransitionBackToStart()
    {
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene("MainMenuScene");
    }
}
