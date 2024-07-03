using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text interactableText;

    [Header("Transform")]
    [SerializeField] private Transform holdPosition;


    [Header("Scripts")]
    [SerializeField] private PickUpObject pickUpObject;
    [SerializeField] private UIController uI;
    [SerializeField] private Battery battery;

    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;


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
        Debug.Log("GameManger is popping");
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
                Debug.Log("First playthrough");
                GrabAllTheTools();
                break;

            case "MainMenuScene":
                Debug.Log("MainMenu");
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
        holdPosition = GameObject.Find("HoldPosition").GetComponent<Transform>();
        playerAnimator = GameObject.FindWithTag("Body").GetComponent<Animator>();
    }

    private IEnumerator TransitionBackToStart()
    {
        yield return new WaitForSeconds(7.5f);
  
        SceneManager.LoadScene("MainMenuScene");
       
    }

    public Transform ReturnHoldPosition()
    {
        return this.holdPosition;
    }

    public Animator ReturnAnimator()
    {
        return this.playerAnimator;
    }    
}