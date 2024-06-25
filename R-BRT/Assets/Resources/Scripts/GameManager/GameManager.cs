using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] bool hasJetPack;
    [SerializeField] bool hasStealth;
    [SerializeField] bool playerIsSpotted;

    [Header("Game Objects")]
    [SerializeField] GameObject player;
 

    [Header("GameManager Instance")]
    public static GameManager Instance;

    [Header("Scripts")]
    [SerializeField] private DetectionMeter detectionMeter;
    [SerializeField] private PlayerController playerController;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This makes the GameObject persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>(); 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            player = GameObject.FindWithTag("Player");
            detectionMeter = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>();
            playerIsSpotted = false;
            playerController.SetCameraLock(false);
        }
        else if (scene.name == "ChooseYourFriend")
        {
            Time.timeScale = 1;
            StartCoroutine(TransitionBackToStart());
        }
        if (scene.name == "SaveTheWorld")
        {
            Time.timeScale = 1;
            StartCoroutine(TransitionBackToStart());
        }
        if (scene.name == "Player_Enemy_TestScene")
        {
            Time.timeScale = 1.0f;
            player = GameObject.FindWithTag("Player");
            detectionMeter = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>();
        }
    }


    public void SetJetPackStatus(bool value)
    {
        hasJetPack = value;
    }

    public bool CanUseJetPack()
    {
        return this.hasJetPack;
    }

    public void SetStealthStatus(bool value)
    {
        hasStealth = value;
    }

    public bool CanUseStealth()
    {
        return this.hasStealth;
    }

    public GameObject ReturnPlayer()
    {
        return this.player;
    }

    public DetectionMeter ReturnDetectionMeter()
    {
        return this.detectionMeter;
    }

    public bool ReturnPlayerSpotted()
    {
        return this.playerIsSpotted;
    }

    public void SetPlayerIsSpotted(bool value)
    {
           this.playerIsSpotted = value; 
    }

    IEnumerator TransitionBackToStart()
    {
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene("MainMenuScene");
    }
}
