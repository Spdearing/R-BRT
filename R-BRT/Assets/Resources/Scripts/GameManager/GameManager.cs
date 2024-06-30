using System.Collections;
using System.Collections.Generic;
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

    [Header("Game Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dialogueTriggerOne;
    [SerializeField] private GameObject dialogueTriggerTwo;
    [SerializeField] private GameObject enemyNumberOne;

    [Header("Transforms")]
    [SerializeField] private Transform friendLocation;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform enemyOneTransform;


    [Header("GameManager Instance")]
    public static GameManager Instance;

    [Header("Scripts")]
    [SerializeField] private DetectionMeter detectionMeter;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GroundBotSpawner groundBotSpawner;

    void Awake()
    {
        Debug.Log("GameManager Awake");

        if (Instance == null)
        {
            firstPlaythrough = true;
            firstDialogueHit = false;
            secondDialogueHit = false;
            Instance = this;
            DontDestroyOnLoad(gameObject); // This makes the GameObject persistent across scenes
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
            Debug.Log("GameManager Instance Created");
        }
        else
        {
            Debug.Log("Duplicate GameManager Destroyed");
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

    void Start()
    {
        Debug.Log("GameManager Start");
    }

    void OnDestroy()
    {
        Debug.Log("GameManager OnDestroy");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        switch (scene.name)
        {
            case "GameScene":
                if(firstPlaythrough && !firstDialogueHit)
                {
                    HandleGameSceneLoad();
                    playerController.SetPlayerActivity(false);
                }
                else
                {
                    LoadLevel();
                }
                
                break;
            case "ChooseYourFriend":
            case "SaveTheWorld":
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

        Debug.Log("Initializing GameScene");
        InitializePlayerAndDetectionMeter();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        friendLocation = GameObject.Find("S-4MTiredShowcase").GetComponent<Transform>();
        playerIsSpotted = false;

        if (firstPlaythrough)
        {
            
            Debug.Log("First playthrough setup");
            StartCoroutine(SmoothCameraRotationToFriend(mainCamera, friendLocation.position, 2));
            playerController.SetCameraLock(true);
            dialogueTriggerOne.SetActive(true);
        }
    }
    
    private void LoadLevel()
    {
        InitializePlayerAndDetectionMeter();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        friendLocation = GameObject.Find("S-4MTiredShowcase").GetComponent<Transform>();
        playerIsSpotted = false;
    }

    private void InitializePlayerAndDetectionMeter()
    {
        Debug.Log("Initializing Player and Detection Meter");
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        detectionMeter = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>();
    }

    private IEnumerator SmoothCameraRotationToFriend(Transform cameraTransform, Vector3 targetPosition, float duration)
    {
        Debug.Log("Starting Smooth Camera Rotation");
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
        Debug.Log("Smooth Camera Rotation Completed");
    }

    private IEnumerator SmoothCameraRotationToFirstEnemy(Transform cameraTransform, Vector3 targetPosition, float duration)
    {
        Debug.Log("Starting Smooth Camera Rotation");
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
        Debug.Log("Smooth Camera Rotation Completed");
    }

    public void StartSecondDialogue()
    {
        enemyOneTransform = enemyNumberOne.transform;
        StartCoroutine(SmoothCameraRotationToFirstEnemy(mainCamera, enemyOneTransform.position, 2));
        playerController.SetCameraLock(true);
        dialogueTriggerOne.SetActive(true);

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
        dialogueTriggerOne.SetActive (value);
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
        Debug.Log("Transitioning Back to Start");
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene("MainMenuScene");
    }
}
