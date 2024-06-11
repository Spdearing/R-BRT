using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //[SerializeField] RockCollision rockCollision;
    //[SerializeField] PickUpObject rock;

    [SerializeField] bool hasJetPack;
    [SerializeField] bool hasStealth;

    [SerializeField] GameObject player;
    [SerializeField] DetectionMeter detectionMeter;

    [SerializeField] static GameManager instance;

    [SerializeField] private AllDirectionRaycast allDirectionRaycast;
    
    public static GameManager Instance;



    void Awake()
    {
        // Implement Singleton pattern
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
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            player = GameObject.FindWithTag("Player");
            detectionMeter = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>();
            //rockCollision = GameObject.Find("Rocks").GetComponent<RockCollision>();
            //rock = GameObject.Find("Rocks").GetComponent<PickUpObject>();
        }
        else if(scene.name == "ChooseYourFriend")
        {
            Time.timeScale = 1;
            StartCoroutine(TransitionBackToStart());
        }
        if(scene.name == "SaveTheWorld")
        {
            Time.timeScale = 1;
            StartCoroutine(TransitionBackToStart());
        }
        if (scene.name == "Player_Enemy_TestScene")
        {
            player = GameObject.FindWithTag("Player");
            detectionMeter = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>();
            //rockCollision = GameObject.Find("Rocks").GetComponent<RockCollision>();
            //rock = GetComponent<PickUpObject>();
            allDirectionRaycast = GameObject.Find("Rocks").GetComponent<AllDirectionRaycast>();
        }
    }

        public void SendOutNoise()
    {

        allDirectionRaycast.CastRaysInAllDirections();
        //StartCoroutine(EnableSoundForDuration());
    }
    IEnumerator EnableSoundForDuration()
    {
        
        allDirectionRaycast.enabled = true;

        
        yield return new WaitForSeconds(.5f);

        
        allDirectionRaycast.enabled = false;
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


    IEnumerator TransitionBackToStart()
    {
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene("MainMenuScene");
    }
}
