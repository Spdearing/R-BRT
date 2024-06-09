using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] RockCollision rockCollision;
    [SerializeField] PickUpObject rock;

    [SerializeField] bool hasJetPack;
    [SerializeField] bool hasStealth;

    [SerializeField] GameObject player;
    [SerializeField] DetectionMeter detectionMeter;

    [SerializeField] static GameManager instance;

    [SerializeField] private AllDirectionRaycast allDirectionRaycast;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Look for an existing instance in the scene
                instance = FindObjectOfType<GameManager>();

                // If no instance exists, create a new one
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GameManager");
                    instance = singletonObject.AddComponent<GameManager>();

                    // Ensure that the GameManager persists across scene changes
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }



    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        detectionMeter = GameObject.Find("EnemyDetectionManager").GetComponent<DetectionMeter>();
        rockCollision = GameObject.Find("Rocks").GetComponent<RockCollision>();
        rock = GetComponent<PickUpObject>();
    }

    public void SendOutNoise()
    {
        StartCoroutine(EnableSoundForDuration());
    }
    IEnumerator EnableSoundForDuration()
    {
        // Enable the desired script
        allDirectionRaycast.enabled = true;

        // Wait for the specified duration
        yield return new WaitForSeconds(.1f);

        // Disable the script after the duration
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
}
