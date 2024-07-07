using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [Header("GameObjects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject phoenixChipMenu;
    [SerializeField] private GameObject loreEntry;
    [SerializeField] private GameObject loreEntryMenu;
    [SerializeField] private GameObject battery;
    [SerializeField] private GameObject fuelMeter;

    [Header("SkinnedMeshRenderer")]
    [SerializeField] private SkinnedMeshRenderer skinMeshRenderer1;
    [SerializeField] private SkinnedMeshRenderer skinMeshRenderer2;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text interactableUIText;
    [SerializeField] private TMP_Text interactableBatteryText;

    [Header("Transform")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform friendLocation;


    [Header("Scripts")]
    [SerializeField] private PhoenixChipDecision phoenixChipDecision;
    [SerializeField] private Battery batteryScript;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private PlayerDetectionState playerDetectionState;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAbilities ability;
    [SerializeField] private PlayerRaycast playerRaycast;
    [SerializeField] private EnemyProximityCheck enemyProximity;
    [SerializeField] private FlyingBotSpawner flyingBotSpawner;
    [SerializeField] private GroundBotSpawner groundBotSpawner;
    [SerializeField] private SpiderBotSpawner spiderBotSpawner;
    [SerializeField] private SceneActivity sceneActivity;
    [SerializeField] private InvisibilityCloak invisibilityCloak;
    [SerializeField] private DetectionMeter detection;


    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;

    [Header("Image")]
    [SerializeField] private Image detectionMeter;
    [SerializeField] private Image invisibilityVisualMeter;
    [SerializeField] private Image invisibilityVisualEmpty;
    [SerializeField] private Image invisibilityVisualAmount;

    [Header("Floats")]
    [SerializeField] float startingDetection;
    [SerializeField] float detectionIncrement;
    [SerializeField] float maxDetection;

    [Header("Audio Source")]
    [SerializeField] AudioSource activateInvisibility;
    [SerializeField] AudioSource invisibilityDuration;
    [SerializeField] AudioSource walkingSound;
    [SerializeField] AudioSource jumpingSound;

    [Header("Slider")]
    [SerializeField] Slider fuelMeterSlider;

    [Header("Capsule Collider")]
    [SerializeField] CapsuleCollider playerCollider;
    





    private void Awake()
    {
        //GM not destroyed
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        switch (scene.name)
        {
            case "GameScene":
                
                GrabAllTheTools();
                break;

            case "MainMenuScene":
                
                break;

            case "SamDies":
            case "VictorySamLives":
                Time.timeScale = 1;
                {
                    StartCoroutine(TransitionBackToStart());
                }
                break;

            default:
                Debug.LogWarning($"Scene '{scene.name}' not handled in OnSceneLoaded");
                break;
        }
    }

    public void GrabAllTheTools()
    {
        player = GameObject.FindWithTag("Player");
        playerCollider = GameObject.Find("Player").GetComponentInChildren<CapsuleCollider>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerCameraTransform = playerCamera.transform;
        playerRaycast = GameObject.FindWithTag("MainCamera").GetComponent<PlayerRaycast>();
        flyingBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<FlyingBotSpawner>();
        groundBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<GroundBotSpawner>();
        spiderBotSpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<SpiderBotSpawner>();
        friendLocation = GameObject.Find("S-4MTiredShowcase")?.transform;
        ability = GameObject.Find("Player").GetComponent<PlayerAbilities>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerDetectionState = GameObject.Find("Player").GetComponent<PlayerDetectionState>();
        enemyProximity = GameObject.Find("Player").GetComponent<EnemyProximityCheck>();
        holdPosition = GameObject.Find("HoldPosition").GetComponent<Transform>();
        playerAnimator = GameObject.FindWithTag("Body").GetComponent<Animator>();
        skinMeshRenderer1 = GameObject.Find("LeftArm_RightArm5").GetComponent<SkinnedMeshRenderer>();
        skinMeshRenderer2 = GameObject.Find("LeftArm_RightArm5 (Copy)").GetComponent<SkinnedMeshRenderer>();
        detectionMeter = GameObject.Find("DetectionAmount").GetComponent<Image>();
        invisibilityVisualAmount = GameObject.Find("InvisibleAmount").GetComponent<Image>();
        invisibilityVisualEmpty = GameObject.Find("InvisibleEmpty").GetComponent<Image>();
        invisibilityVisualMeter = GameObject.Find("InvisibleMeter").GetComponent<Image>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        gameOverScreen = GameObject.Find("Canvas").GetComponentInChildren<GameOverScreen>();
        sceneActivity = GameObject.FindWithTag("Canvas").GetComponent<SceneActivity>();
        invisibilityCloak = GameObject.Find("LeftArm_RightArm5").GetComponent<InvisibilityCloak>();
        detection = GameObject.FindWithTag("DetectionMeter").GetComponent<DetectionMeter>();
        interactableUIText = GameObject.FindWithTag("InteractableUIText").GetComponent<TMP_Text>();
        interactableBatteryText = GameObject.FindWithTag("InteractableText").GetComponent<TMP_Text>();
        phoenixChipDecision = GameObject.Find("Canvas").GetComponent<PhoenixChipDecision>();
        batteryScript = GameObject.FindWithTag("Battery").GetComponent<Battery>();
        battery = GameObject.FindWithTag("Battery");
        phoenixChipMenu = GameObject.FindWithTag("PhoenixChipMenu");
        loreEntry = GameObject.FindWithTag("LoreEntry");
        fuelMeter = GameObject.FindWithTag("FuelMeter");
        activateInvisibility = GameObject.Find("ActivateInvisibility").GetComponent<AudioSource>();
        invisibilityDuration = GameObject.Find("InvisibilityDuration").GetComponent<AudioSource>();
        walkingSound = GameObject.Find("WalkingSound").GetComponent<AudioSource>();
        jumpingSound = GameObject.Find("JumpSound").GetComponent<AudioSource>();
        fuelMeterSlider = GameObject.Find("Fuel Meter").GetComponent<Slider>();

    }

    private IEnumerator TransitionBackToStart()
    {
        yield return new WaitForSeconds(7.5f);
  
        SceneManager.LoadScene("MainMenuScene");
       
    }

    public CapsuleCollider ReturnPlayerCollider()
    {
        return this.playerCollider;
    }

    public Slider ReturnFuelMeterSlider()
    {
        return this.fuelMeterSlider;
    }

    public AudioSource ReturnWalkingSound()
    {
        return this.walkingSound;
    }
    public AudioSource ReturnJumpingSound()
    {
        return this.jumpingSound;
    }
    public AudioSource ReturnActivatingInvisibilitySound()
    {
        return this.activateInvisibility;
    }
    public AudioSource ReturnInvisibilityDurationSound()
    {
        return this.invisibilityDuration;
    }

    public TMP_Text ReturnInteractableText()
    {
        return this.interactableUIText;
    }

    public TMP_Text ReturnInteractableBatteryText()
    {
        return this.interactableBatteryText;
    }

    public PlayerRaycast ReturnPlayerRaycast() 
    {
        return this.playerRaycast;
    }

    public Battery ReturnBatteryScript() 
    {
        return this.batteryScript;
    }

    public PhoenixChipDecision ReturnPhoenixChipDecision()
    {
        return this.phoenixChipDecision;
    }

    public LoreEntry ReturnLoreEntry()
    {
        return this.gameObject;
    }

    public DetectionMeter ReturnDetectionMeter()
    {
        return this.detection;
    }

    public InvisibilityCloak ReturnInvisibilityCloak()
    {
        return this.invisibilityCloak;
    }

    public SceneActivity ReturnSceneActivity()
    {
        return this.sceneActivity;
    }

    public PlayerAbilities ReturnPlayerAbilities()
    {
        return this.ability;
    }

    public EnemyProximityCheck ReturnEnemyProximityCheck()
    {
        return this.enemyProximity;
    }

    public SpiderBotSpawner ReturnSpiderBotSpawner()
    {
        return this.spiderBotSpawner;
    }

    public GroundBotSpawner ReturnGroundBotSpawner()
    {
        return this.groundBotSpawner;
    }

    public FlyingBotSpawner ReturnFlyingBotSpawner()
    {
        return this.flyingBotSpawner;
    }

    public PlayerController ReturnPlayerController()
    {
        return this.playerController;
    }

    public PlayerDetectionState ReturnPlayerDetectionState()
    {
        return this.playerDetectionState;
    }

    public GameObject ReturnFuelMeter()
    {
        return this.fuelMeter;
    }

    public GameObject ReturnPhoenixChipMenu()
    {
        return this.phoenixChipMenu;
    }

    public GameObject ReturnLoreEntryMenu()
    {
        return this.loreEntryMenu;
    }

    public GameObject ReturnBatteryObject()
    {
        return this.battery;
    }

    public GameObject ReturnPlayer()
    {
        return this.player;
    }

    public GameObject ReturnPlayerCamera() 
    {
        return this.playerCamera;
    }

    public Transform ReturnFriendsLocation()
    {
        return this.friendLocation;
    }

    public Transform ReturnPlayerTransform()
    {
        return this.playerTransform;
    }

    public Image ReturnDetectionAmountImage()
    {
        return this.detectionMeter;
    }

    public Image ReturnInvisibilityMeterAmount()
    {
        return this.invisibilityVisualAmount;
    }

    public Image ReturnInvisibilityMeter()
    {
        return this.invisibilityVisualMeter;
    }

    public Image ReturnInvisibilityMeterEmpty()
    {
        return this.invisibilityVisualEmpty;
    }

    public Transform ReturnHoldPosition()
    {
        return this.holdPosition;
    }

    public Transform ReturnCameraTransform()
    {
        return this.playerCameraTransform;
    }

    public Animator ReturnAnimator()
    {
        return this.playerAnimator;
    }   
    
    public SkinnedMeshRenderer ReturnRendererOne()
    {
        return this.skinMeshRenderer1;
    }
    public SkinnedMeshRenderer ReturnRendererTwo()
    {
        return this.skinMeshRenderer2;
    }

    public GameOverScreen ReturnGameOver()
    {
        return this.gameOverScreen;
    }

}