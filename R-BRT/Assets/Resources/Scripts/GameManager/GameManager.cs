using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region//Game Objects
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
    [SerializeField] private GameObject dialogueTenHitBox;
    [SerializeField] private GameObject dialogueElevenHitBox;
    [SerializeField] private GameObject dialogueTwelveHitBox;
    [SerializeField] private GameObject invisibilityMeter;
    [SerializeField] private GameObject stealthBlockade;
    [SerializeField] private GameObject jetPackBlockade;
    [SerializeField] private GameObject[] checkPoints;
    [SerializeField] private GameObject startingSpawnPoint;
    [SerializeField] private GameObject doorOpenOne;
    [SerializeField] private GameObject doorOpenTwo;
    [SerializeField] private GameObject doorOpenThree;//janitor closet
    [SerializeField] private GameObject doorClosedOne;
    [SerializeField] private GameObject doorClosedTwo;
    [SerializeField] private GameObject doorClosedThree;//Janitors closet
    [SerializeField] private GameObject crouchVolume;
    [SerializeField] private GameObject invisVolume;
    #endregion

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

    [Header("Transform")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerOtherTransform;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform friendLocation;
    [SerializeField] private Transform[] checkPointLocations;
    

    [Header("Vector3")]
    [SerializeField] private Vector3 newSpawnPoint;
    [SerializeField] private Vector3 startingSpawnLocation;
    

    


    //list of spawn points that are added when a checkpoint is hit
    [SerializeField] private List<Transform> playerSpawnLocations = new List<Transform> { };

    #region//Scripts
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
    [SerializeField] private MainMenuController mainMenuController;
    #endregion


    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator interactBoxAnimator;
    [SerializeField] private Animator interactEAnimator;

    #region//Images
    [Header("Image")]
    [SerializeField] private Image detectionMeter;
    [SerializeField] private Image invisibilityVisualMeter;
    [SerializeField] private Image invisibilityVisualEmpty;
    [SerializeField] private Image invisibilityVisualAmount;
    [SerializeField] private Image fuelMeterSlider;
    #endregion

    #region//Floats
    [Header("Floats")]
    [SerializeField] private float startingDetection;
    [SerializeField] private float detectionIncrement;
    [SerializeField] private float maxDetection;
    #endregion


    #region//Audio Sources
    [Header("Audio Source")]
    [SerializeField] private AudioSource activateInvisibility;
    [SerializeField] private AudioSource invisibilityDuration;
    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private AudioSource sprintingSound;
    [SerializeField] private AudioSource jumpingSound;
    #endregion

    [Header("Slider")]
    //changed fuelMeterSlider from a Slider to an Image

    [Header("Capsule Collider")]
    [SerializeField] CapsuleCollider playerCollider;

    [Header("Bools")]
    [SerializeField] private bool[] loreEntries;
    [SerializeField] private bool[] checkPointsHit;
    [SerializeField] private bool newGame;
    [SerializeField] private bool playerCaught;
    [SerializeField] private bool invisibilityUnlocked;
    [SerializeField] private bool jetpackUnlocked;
    [SerializeField] private bool hasPickedAbility;
    [SerializeField] private bool playerHasClearedFirstFloor;

    [Header("Buttons")]
    [SerializeField] private Button[] loreButtons;

    [Header("Flash Light")]
    [SerializeField] private Light flashLight;

    [Header("Index For Ability Dialogue")]
    [SerializeField] private int index;
    [SerializeField] private int currentCheckPointIndex = 0;

    private void Awake()
    {
        //GM not destroyed
        if (instance == null)
        {
            newGame = true;
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

    private void Update()
    {
        DEVCHEATS();
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        switch (scene.name)
        {
            case "GameScene":
                if (newGame)
                {
                    hasPickedAbility = false;
                    playerHasClearedFirstFloor = false;
                    jetpackUnlocked = false;
                    invisibilityUnlocked = false;
                    playerCaught = false;
                    dialogueCheckPoint = "First Dialogue";
                    GrabAllTheTools();
                    startingSpawnLocation = startingSpawnPoint.transform.position;
                    player.transform.position = startingSpawnLocation;
                }
                else if (!newGame)
                {
                    playerCaught = false;
                    GrabAllTheTools();

                    if (newSpawnPoint != new Vector3(0, 0, 0))
                    {
                        Debug.Log("About to set new spawn location");
                        player.transform.position = newSpawnPoint;
                    }
                    else
                    {
                        Debug.Log("Player did not have a new spawn Location");
                        startingSpawnLocation = startingSpawnPoint.transform.position;
                        player.transform.position = startingSpawnLocation;
                    }
                }
        

                break;

            case "MainMenuScene":
                mainMenuController = GameObject.Find("Canvas").GetComponent<MainMenuController>();

                break;

            case "SamDies":
            case "VictorySamLives":
                break;

            default:
                Debug.LogWarning($"Scene '{scene.name}' not handled in OnSceneLoaded");
                break;
        }
    }


    public void RespawnPlayer()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GrabAllTheTools()
    {
        startingSpawnPoint = GameObject.Find("StartingSpawnPoint");

        playerDialogueCheckPoints = new string[] { "First Dialogue", "Second Dialogue", "Third Dialogue", "Fourth Dialogue", "Fifth Dialogue", "Sixth Dialogue", "Seventh Dialogue", "Eighth Dialogue" };

        abilityChosen = new string[] { "Stealth", "Jetpack", "Stealth2", "Jetpack2", "Stealth3", "Jetpack3" };

        loreButtons = new Button[4];
        loreButtons[0] = GameObject.Find("LoreEntry1").GetComponent<Button>();
        loreButtons[1] = GameObject.Find("LoreEntry2").GetComponent<Button>();
        loreButtons[2] = GameObject.Find("LoreEntry3").GetComponent<Button>();
        loreButtons[3] = GameObject.Find("LoreEntry4").GetComponent<Button>();

        checkPoints = new GameObject[6];
        checkPoints[0] = GameObject.Find("After Lobby");
        checkPoints[1] = GameObject.Find("In Front Of Janitors Closet");
        checkPoints[2] = GameObject.Find("Top Of Elevator Spawn");
        checkPoints[3] = GameObject.Find("Top Of Stairs Spawn");
        checkPoints[4] = GameObject.Find("Second Broken Room");
        checkPoints[5] = GameObject.Find("Before Jetpack Puzzle");

        checkPointLocations = new Transform[6];
        checkPointLocations[0] = checkPoints[0].transform;
        checkPointLocations[1] = checkPoints[1].transform;
        checkPointLocations[2] = checkPoints[2].transform;
        checkPointLocations[3] = checkPoints[3].transform;
        checkPointLocations[4] = checkPoints[4].transform;
        checkPointLocations[5] = checkPoints[5].transform;

        loreEntries = new bool[4];
        loreEntries[0] = false;
        loreEntries[1] = false;
        loreEntries[2] = false;
        loreEntries[3] = false;

        checkPointsHit = new bool[6];
        checkPointsHit[0] = false;
        checkPointsHit[1] = false;
        checkPointsHit[2] = false;
        checkPointsHit[3] = false;
        checkPointsHit[4] = false;
        checkPointsHit[5] = false;


        player = GameObject.Find("Player");
        playerCollider = GameObject.Find("Player").GetComponentInChildren<CapsuleCollider>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerCameraTransform = playerCamera.transform;
        playerOtherTransform = GameObject.Find("RenderInFrontCam").GetComponent<Transform>();
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
        dialogueTenHitBox = GameObject.Find("TenthDialogueEncounter");
        dialogueElevenHitBox = GameObject.Find("EleventhDialogueEncounter");
        dialogueTwelveHitBox = GameObject.Find("TwelthDialogueEncounter");
        flashLight = GameObject.Find("FlashLight").GetComponent<Light>();
        stealthBlockade = GameObject.Find("StealthBlockade");
        jetPackBlockade = GameObject.Find("ElevatorBlockade");
        interactBoxAnimator = GameObject.Find("Crosshair").GetComponent<Animator>();
        interactEAnimator = GameObject.Find("InteractE").GetComponent<Animator>();
        stealthBlockade.SetActive(false);
        jetPackBlockade.SetActive(false);
        fuelMeter.SetActive(false);
        invisibilityMeter.SetActive(false);
        doorClosedOne = GameObject.Find("DoorClosedOne");
        doorClosedOne.SetActive(false);
        doorClosedTwo = GameObject.Find("DoorClosedTwo");
        doorClosedTwo.SetActive(false);
        doorClosedThree = GameObject.Find("DoorClosedJanitorsCloset");
        doorClosedThree.SetActive(false);
        doorOpenOne = GameObject.Find("DoorOpenOne");
        doorOpenOne.SetActive(true);
        doorOpenTwo = GameObject.Find("DoorOpenTwo");
        doorOpenTwo.SetActive(true);
        doorOpenThree = GameObject.Find("DoorOpenJanitorsCloset");
        doorOpenThree.SetActive(true);
        crouchVolume = GameObject.Find("CrouchVolume");
        invisVolume = GameObject.Find("InvisVolume");
        crouchVolume.SetActive(false);
        invisVolume.SetActive(false);
    }

    public void PlayerHasJetpack()
    {
        playerController.SetPlayerActivity(true);
        playerController.SetCameraLock(false);
        ability.SetJetPackUnlock(true);
        fuelMeter.SetActive(true);
        stealthBlockade.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PlayerHasInvisibility()
    {
        
        playerController.SetPlayerActivity(true);
        playerController.SetCameraLock(false);
        ability.SetInvisibilityUnlock(true);
        invisibilityMeter.SetActive(true);
        jetPackBlockade.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CheckSpawnLocation(int index)
    {
        foreach (bool checkPoint in checkPointsHit)
        {
            if (checkPointsHit[index] == true)
            {
                playerTransform = checkPointLocations[index];
            }
        }
    }

    #region//Setting Variables

    public void SetPlayerHasClearedHallway(bool value)
    {
        playerHasClearedFirstFloor = value;
    }

    public void SetHasPickedAbility(bool value)
    {
        hasPickedAbility = value;
    }
    public void SetInvisibilityStatus(bool value)
    {
        invisibilityUnlocked = value;
    }

    public void SetJetpackStatus(bool value)
    {
        jetpackUnlocked = value;
    }

    public void SetPlayerSpawnLocation(int index)
    {
        playerTransform = checkPointLocations[index];
    }

    public void SetCheckPointHit(int index)
    {
        checkPointsHit[index] = true;
    }
    public void SetIndexForAbilityChoice(int value)
    {
        index = value;
    }
    public void SetPlayerAbilityChoice(int index)
    {
        abilityChosen[index] = abilityChosen[index];
    }
    public void SetDialogueCheckPoint(string checkPointName)
    {
        dialogueCheckPoint = checkPointName;
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

    public void SetNewGameStatus(bool value)
    {
        newGame = value;
    }

    public void SetPlayerCaughtStatus(bool value)
    {
        playerCaught = value;
    }
    #endregion

    #region//Returning Strings

    public string ReturnPlayerDialogueCheckPoints(int index)
    {
        return this.playerDialogueCheckPoints[index];
    }

    public string ReturnPlayerDialogueAbilityChoice()
    {
        return this.abilityChosen[index];
    }

    public string ReturnDialogueCheckPoint()
    {
        return this.dialogueCheckPoint;
    }
    #endregion

    #region //Return GameObjects

    public GameObject ReturnInvisibilityVolume()
    {
        return this.invisVolume;
    }

    public GameObject ReturnCrouchVolume()
    {
        return this.crouchVolume;
    }
    public GameObject ReturnStealthBlockade()
    {
        return this.stealthBlockade;
    }

    public GameObject ReturnPlayerCheckPoint(int index)
    {
        return this.checkPoints[index];
    }

    public GameObject ReturnElevatorBlockade()
    {
        return this.jetPackBlockade;
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
    public GameObject ReturnDialogueNineHitBox()
    {
        return this.dialogueNineHitBox;
    }

    public GameObject ReturnDialogueTenHitBox()
    {
        return this.dialogueTenHitBox;
    }

    public GameObject ReturnDialogueElevenHitBox()
    {
        return this.dialogueElevenHitBox;
    }
    public GameObject ReturnDialogueTwelthHitBox()
    {
        return this.dialogueTwelveHitBox;
    }

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

    public GameObject ReturnLoreEntry()
    {
        return this.loreEntry;
    }
    #endregion

    #region//ReturningAudio Sources
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
    #endregion

    #region//Returning Transforms
    public Transform ReturnFriendsLocation()
    {
        return this.friendLocation;
    }

    public Transform ReturnPlayerOtherCamera()
    {
        return this.playerOtherTransform;
    }

    public Transform ReturnPlayerTransform()
    {
        return this.playerTransform;
    }

    public Transform ReturnHoldPosition()
    {
        return this.holdPosition;
    }

    public Transform ReturnCameraTransform()
    {
        return this.playerCameraTransform;
    }
    #endregion

    #region//Returning UI Elements

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

    public Image ReturnFuelMeterSlider()
    {
        return this.fuelMeterSlider;
    }
    //public TMP_Text ReturnInteractableText()
    //{
    //    return this.interactableUIText;
    //}

    //public TMP_Text ReturnInteractableBatteryText()
    //{
    //    return this.interactableBatteryText;
    //}

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

    public GameOverScreen ReturnGameOver()
    {
        return this.gameOverScreen;
    }
    public FirstDialogueFunctionality ReturnFirstDialogueFunctionality()
    {
        return this.firstDialogueFunctionality;
    }
    public Animator ReturnInteractBoxAnim()
    {
        return this.interactBoxAnimator;
    }
    public Animator ReturnInteractEAnim()
    {
        return this.interactEAnimator;
    }

    #endregion

    #region//Returning Spawners
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
    #endregion

    #region//Returning Player Components
    public CapsuleCollider ReturnPlayerCollider()
    {
        return this.playerCollider;
    }

    public PlayerController ReturnPlayerController()
    {
        return this.playerController;
    }

    public PlayerDetectionState ReturnPlayerDetectionState()
    {
        return this.playerDetectionState;
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
    #endregion

    #region//Returning Bools


    public bool CheckIfPickedUpAbility()
    {
        return this.hasPickedAbility;
    }
    public bool ReturnInvisibilityStatus()
    {
        return this.invisibilityUnlocked;
    }
    public bool ReturnJetpackStatus()
    {
        return this.jetpackUnlocked;
    }
    public bool ReturnNewGameStatus()
    {
        return this.newGame;
    }

    public bool ReturnPlayerCaughtStatus()
    {
        return this.playerCaught;
    }
    #endregion


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

    public void AddSpawnPoint(Transform transform)
    {
        if(currentCheckPointIndex < checkPoints.Length)
        {
            playerSpawnLocations.Insert(0, transform);
            newSpawnPoint = playerSpawnLocations[0].position;
            checkPoints[currentCheckPointIndex].SetActive(false);
            currentCheckPointIndex++;
        }
       
    }

    public void DestroyGameObject(GameObject value)
    {
        Destroy(value.gameObject);
    }

    public Light ReturnFlashLight()
    {
        return this.flashLight;
    }

    void DEVCHEATS()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            invisibilityUnlocked = true;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            jetpackUnlocked = true;
        }
    }

    public MainMenuController ReturnMainMenuController()
    {
        return this.mainMenuController;
    }

    public void CloseOffTheStairs()
    {
        doorClosedOne.SetActive(true);
        doorClosedTwo.SetActive(true);
        doorOpenOne.SetActive(false);
        doorOpenTwo.SetActive(false);
    }

    public void ShutJanitorsCloset()
    {
        doorOpenThree.SetActive(false);
        doorClosedThree.SetActive(true);
    }
}