using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlyingBotStateMachine;
using UnityEngine.InputSystem.LowLevel;
using Unity.VisualScripting;

public class EnemyFieldOfView : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerDetectionState playerDetectionState;
    [SerializeField] private PlayerController player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EnemyProximityCheck enemyProximity;
    [SerializeField] private GroundBotHeadMovement groundBotHeadMovement;
    [SerializeField] private FlyingBotStateMachine flyingBotStateMachine;

    [Header("Bools")]
    [SerializeField] private bool playerIsBeingDetected;

    [Header("Enemy")]
    [SerializeField] private GameObject enemy;

    [Header("Transform")]
    [SerializeField] private Transform enemyGrandparentTransform;

    [Header("GameObject")]
    [SerializeField] GameObject flyingBot;



    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyProximity = GameObject.Find("Player").GetComponent<EnemyProximityCheck>();
        playerDetectionState = GameObject.Find("Player").GetComponent<PlayerDetectionState>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerIsBeingDetected = false;
        enemyGrandparentTransform = gameObject.transform.parent.parent;
        flyingBot = Resources.Load<GameObject>("Sam's_Prefabs/FlyinBotFinal");
        flyingBotStateMachine = flyingBot.GetComponent<FlyingBotStateMachine>();
        Debug.Log(enemyGrandparentTransform);

        //if (gameObject.transform.parent.parent != null)
        //{
        //    string parentTag = gameObject.transform.parent.parent.tag;
        //    if (parentTag == "GroundBot" || parentTag == "FlyingBot")
        //    {
        //        enemy = gameObject.transform.parent.parent.gameObject;
        //        enemyGrandparentTransform = gameObject.transform.parent.parent;
        //        Debug.Log(enemy.ToString());
        //    }
        //}
    }

    private void Update()
    {
        DetectingPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !player.ReturnUsingInvisibility())
        {
            gameManager.SetPlayerIsSpotted(true);
            playerIsBeingDetected = true;

            if(gameObject.transform.parent.parent.tag == "GroundBot")
            {
                groundBotHeadMovement.SetPlayerSpotted(true);
            }
            else if (gameObject.transform.parent.tag == "FlyingBot")
            {
                flyingBotStateMachine.ChangeBehavior(FlyingBotStateMachine.FlyingState.scanning);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !player.ReturnUsingInvisibility())
        {
            bool withinRange = enemyProximity.ReturnEnemyWithinRange();
            gameManager.SetPlayerIsSpotted(withinRange);

            if (!withinRange)
            {
                playerIsBeingDetected = false;
                playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
            }
            else
            {
                playerIsBeingDetected = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enemyProximity.ReturnEnemyWithinRange() && !player.ReturnUsingInvisibility())
        {
            gameManager.SetPlayerIsSpotted(false);
            playerIsBeingDetected = false;

            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
            
            if (gameObject.transform.parent.parent.tag == "GroundBot")
            {
                groundBotHeadMovement.SetPlayerSpotted(false);
            }
            else if (gameObject.transform.parent.tag == "FlyingBot")
            {
                flyingBotStateMachine.ChangeBehavior(FlyingBotStateMachine.FlyingState.scanning);
            }
        }
    }

    private void DetectingPlayer()
    {
        if (enemyProximity.ReturnEnemyWithinRange() && gameManager.ReturnPlayerSpotted())
        {
            if (playerIsBeingDetected)
            {
                playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);
                playerIsBeingDetected = false;
            }
        }
    }

    public Transform ReturnThisEnemy()
    {
        return this.enemyGrandparentTransform;
    }
}
