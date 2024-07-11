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
    [SerializeField] private GameObject loreEntry2;
    [SerializeField] private GameObject loreEntry3;
    [SerializeField] private GameObject loreEntry4;
    [SerializeField] private GameObject battery;
    [SerializeField] private GameObject fuelMeter;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject abilityDialogue;
    [SerializeField] private GameObject dialogueTwoHitBox;
    [SerializeField] private GameObject dialogueThreeHitBox;
    [SerializeField] private GameObject dialogueFourHitBox;
    [SerializeField] private GameObject dialogueFiveHitBox;
    [SerializeField] private GameObject dialogueSixHitBox;
    [SerializeField] private GameObject dialogueSevenHitBox;
    [SerializeField] private GameObject dialogueEightHitBox;
    [SerializeField] private GameObject dialogueNineHitBox;
    [SerializeField] private GameObject invisibilityMeter;
    [SerializeField] private GameObject stealthBlockade;
    [SerializeField] private GameObject JetPackBlockade;

    [Header("Dialogue String")]
    [SerializeField] private string dialogueCheckPoint;

    [Header("Strings Array For Dialogues")]
    [SerializeField] private string[] playerDialogueCheckPoints;
    
    [SerializeField] private string[] abilityChosen;

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
    [SerializeField] private FirstDialogueFunctionality firstDialogueFunctionality;


    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;

    [Header("Image")]
    [SerializeField] private Image detectionMeter;
    [SerializeField] private Image invisibilityVisualMeter;
    [SerializeField] private Image invisibilityVisualEmpty;
    [SerializeField] private Image invisibilityVisualAmount;
    [SerializeField] private Image fuelMeterSlider;

    [Header("Floats")]
    [SerializeField] private float startingDetection;
    [SerializeField] private float detectionIncrement;
    [SerializeField] private float maxDetection;

    [Header("Audio Source")]
    [SerializeField] private AudioSource activateInvisibility;
    [SerializeField] private AudioSource invisibilityDuration;
    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private AudioSource sprintingSound;
    [SerializeField] private AudioSource jumpingSound;

    [Header("Slider")]
    //changed fuelMeterSlider from a Slider to an Image

    [Header("Capsule Collider")]
    [SerializeField] CapsuleCollider playerCollider;

    [Header("Bools")]
    [SerializeField] private bool[] loreEntries;
    

    [Header("Buttons")]
    [SerializeField] private Button[] loreButtons;

    [Header("Flash Light")]
    [SerializeField] private Light flashLight;

    [Header("Index For Ability Dialogue")]
    [SerializeField] private int index;
    
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

                dialogueCheckPoint = "First Dialogue";
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
        playerDialogueCheckPoints = new string[] { "First Dialogue", "Second Dialogue", "Third Dialogue", "Fourth Dialogue", "Fifth Dialogue", "Sixth Dialogue", "Seventh Dialogue", "Eighth Dialogue" };
        abilityChosen = new string[] { "Stealth", "Jetpack", "Stealth2", "Jetpack2", "Stealth3", "Jetpack3" };

        loreButtons = new Button[4];
        loreButtons[0] = GameObject.Find("LoreEntry1").GetComponent<Button>();
        loreButtons[1] = GameObject.Find("LoreEntry2").GetComponent<Button>();
        loreButtons[2] = GameObject.Find("LoreEntry3").GetComponent<Button>();
        loreButtons[3] = GameObject.Find("LoreEntry4").GetComponent<Button>();

        loreEntries = new bool[4];
        loreEntries[0] = false;
        loreEntries[1] = false;
        loreEntries[2] = false;
        loreEntries[3] = false;

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
        invisibilityMeter = GameObject.Find("StealthImage");
        invisibilityVisualAmount = GameObject.Find("StealthMeterFill").GetComponent<Image>();
        invisibilityVisualEmpty = GameObject.Find("StealthMeterMask").GetComponent<Image>();
        invisibilityVisualMeter = GameObject.Find("StealthImage").GetComponent<Image>();
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
        loreEntry2 = GameObject.FindWithTag("LoreEntry2");
        loreEntry3 = GameObject.FindWithTag("LoreEntry3");
        loreEntry4 = GameObject.FindWithTag("LoreEntry4");
        fuelMeter = GameObject.FindWithTag("FuelMeter");
        activateInvisibility = GameObject.Find("ActivateInvisibility").GetComponent<AudioSource>();
        invisibilityDuration = GameObject.Find("InvisibilityDuration").GetComponent<AudioSource>();
        walkingSound = GameObject.Find("WalkingSound").GetComponent<AudioSource>();
        sprintingSound = GameObject.Find("SprintingSound").GetComponent<AudioSource>();
        jumpingSound = GameObject.Find("JumpSound").GetComponent<AudioSource>();
        fuelMeterSlider = GameObject.Find("JetpackMeterFill").GetComponent<Image>();
        abilityDialogue = GameObject.Find("AbilityDialogue");
        dialogue = GameObject.Find("DialoguePanel");
        firstDialogueFunctionality = dialogue.GetComponentInChildren<FirstDialogueFunctionality>();
        dialogueTwoHitBox = GameObject.Find("SecondDialogueEncounter");
        dialogueThreeHitBox = GameObject.Find("ThirdDialogueEncounter");
        dialogueFourHitBox = GameObject.Find("FourthDialogueEncounter");
        dialogueFiveHitBox = GameObject.Find("FifthDialogueEncounter");
        dialogueSixHitBox = GameObject.Find("SixthDialogueEncounter");
        dialogueSevenHitBox = GameObject.Find("SeventhDialogueEncounter");
        dialogueEightHitBox = GameObject.Find("EighthDialogueEncounter");
        dialogueNineHitBox = GameObject.Find("NinthDialogueEncounter");
        flashLight = GameObject.Find("FlashLight").GetComponent<Light>();
        stealthBlockade = GameObject.Find("StealthBlockade");

    }

    public void CheckLoreButtonStatus()
    {
        int index = 0;
        foreach (bool activeLore in loreEntries)
        {
            Debug.Log($"Lore Entry {index}: {activeLore}");
            loreButtons[index].interactable = activeLore;
            index++;
        }
    }

    public void SetIndexForAbilityChoice(int value)
    {
        index = value;
    }

    public string ReturnPlayerDialogueCheckPoints(int index)
    {
        return this.playerDialogueCheckPoints[index];
    }

    public string ReturnPlayerDialogueAbilityChoice()
    {
        return this.abilityChosen[index];
    }

    public void SetPlayerAbilityChoice(int index)
    {
        abilityChosen[index] = abilityChosen[index];
    }


    public void DestroyGameObject(GameObject value)
    {
        Destroy(value.gameObject);
    }

    public Light ReturnFlashLight()
    {
        return this.flashLight;
    }

    public string ReturnDialogueCheckPoint()
    {
        return this.dialogueCheckPoint;
    }

    public void SetDialogueCheckPoint(string checkPointName)
    {
        dialogueCheckPoint = checkPointName;
    }

    public GameObject ReturnStealthBlockade()
    {
        return this.stealthBlockade;
    }

    public GameObject ReturnInvisibilityMeterGameObject()
    {
        return this.invisibilityMeter;
    }

    public GameObject ReturnDialogue()
    {
        return this.dialogue;
    }

    public GameObject ReturnAbilityDialogue()
    {
        return this.abilityDialogue;
    }

    public GameObject ReturnDialogueTwoHitBox()
    {
        return this.dialogueTwoHitBox;
    }

    public GameObject ReturnDialogueThreeHitBox()
    {
        return this.dialogueThreeHitBox;
    }

    public GameObject ReturnDialogueFourHitBox()
    {
        return this.dialogueFourHitBox;
    }

    public GameObject ReturnDialogueFiveHitBox()
    {
        return this.dialogueFiveHitBox;
    }
    public GameObject ReturnDialogueSixHitBox()
    {
        return this.dialogueSixHitBox;
    }
    public GameObject ReturnDialogueSevenHitBox()
    {
        return this.dialogueSevenHitBox;
    }
    public GameObject ReturnDialogueEightHitBox()
    {
        return this.dialogueEightHitBox;
    }
    //public GameObject ReturnDialogueNineHitBox()
    //{
    //    return this.dialogueNineHitBox;
    //}

    public GameObject ReturnLoreEntryOneGameObject()
    {
        return this.loreEntry;
    }
    public GameObject ReturnLoreEntryTwoGameObject()
    {
        return this.loreEntry2;
    }
    public GameObject ReturnLoreEntryThreeGameObject()
    {
        return this.loreEntry3;
    }
    public GameObject ReturnLoreEntryFourGameObject()
    {
        return this.loreEntry4;
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

    public Image ReturnFuelMeterSlider()
    {
        return this.fuelMeterSlider;
    }

    public AudioSource ReturnSprintingSound()
    {
        return this.sprintingSound;
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

    public GameObject ReturnLoreEntry()
    {
        return this.loreEntry;
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

    public Image ReturnInvisibilityMeterImage()
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
    public FirstDialogueFunctionality ReturnFirstDialogueFunctionality()
    {
        return this.firstDialogueFunctionality;
    }

    public void SetLoreEntryOne(bool value)
    {
        loreEntries[0] = value; 
    }
    public void SetLoreEntryTwo(bool value)
    {
        loreEntries[1] = value;
    }
    public void SetLoreEntryThree(bool value)
    {
        loreEntries[2] = value;
    }
    public void SetLoreEntryFour(bool value)
    {
        loreEntries[3] = value;
    }


}