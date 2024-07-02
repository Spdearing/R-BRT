using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GroundBotSpawner : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;

    [Header("EnemyGroups")]
    [SerializeField] GameObject[] group1;
    [SerializeField] GameObject[] group2;
    [SerializeField] GameObject[] group3;
    [SerializeField] GameObject[] group4;

    [Header("EnemyGroupLocations")]
    [SerializeField] Transform[] enemyGroupLocations1;
    [SerializeField] Transform[] enemyGroupLocations2;
    [SerializeField] Transform[] enemyGroupLocations3;
    [SerializeField] Transform[] enemyGroupLocations4;

    [Header("References")]
    [SerializeField] GroundBotStateMachine groundBotStateInstance;
    [SerializeField] GroundBotHeadMovement groundBotHeadMovementInstance;
    [SerializeField] EnemyGroundBotFieldOfView enemyGroundBotFieldViewInstance;
    [SerializeField] GroundBotAIMovement groundBotAIMovementInstance;

    [Header("Nav Mesh Agent")]
    [SerializeField] private NavMeshAgent groundBotAI;

    [Header("Script")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SceneActivity sceneActivity; 

    // Start is called before the first frame update
    void Start()
    {
        sceneActivity = GameObject.FindWithTag("Canvas").GetComponent<SceneActivity>();
        gameManager = GameManager.Instance;
        enemyPrefab = Resources.Load<GameObject>("Sam's_Prefabs/groundBotDone");
        group1 = new GameObject[1];
        group2 = new GameObject[4];
        group3 = new GameObject[6];
        group4 = new GameObject[8];
        SpawnGroup1();
        SpawnGroup2();
        SpawnGroup3(); 
        SpawnGroup4();

    }

    public void SpawnGroup1()
    {
        for (int i = 0; i < group1.Length; i++)
        {
            if (i < enemyGroupLocations1.Length)
            {
                GameObject enemy = Instantiate(enemyPrefab, enemyGroupLocations1[i].position, enemyGroupLocations1[i].rotation);
                group1[i] = enemy;
                GroundBotHeadMovement headMovement = enemy.GetComponent<GroundBotHeadMovement>();
                GroundBotStateMachine stateMachine = enemy.GetComponent<GroundBotStateMachine>();
                EnemyGroundBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemyGroundBotFieldOfView>();

                if (headMovement != null) groundBotHeadMovementInstance = headMovement;
                if (stateMachine != null) groundBotStateInstance = stateMachine;
                if (fieldOfView != null) enemyGroundBotFieldViewInstance = fieldOfView;

                enemy.name = "GroundBotGroup1";
                enemy.tag = "GroundBot";
                sceneActivity.SetEnemyOne(group1[0]);
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }

    public void SpawnGroup2()
    {
        for (int i = 0; i < group2.Length; i++)
        {
            if (i < enemyGroupLocations2.Length)
            {

                GameObject enemy = Instantiate(enemyPrefab, enemyGroupLocations2[i].position, enemyGroupLocations2[i].rotation);
                group2[i] = enemy;
                GroundBotHeadMovement headMovement = enemy.GetComponent<GroundBotHeadMovement>();
                GroundBotStateMachine stateMachine = enemy.GetComponent<GroundBotStateMachine>();
                EnemyGroundBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemyGroundBotFieldOfView>();
                enemy.AddComponent<NavMeshAgent>();
                NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
                navMeshAgent.baseOffset = .5f;
                navMeshAgent.height = 1.0f;
                enemy.AddComponent<GroundBotAIMovement>();
                GroundBotAIMovement groundBotAIMovement = enemy.GetComponent<GroundBotAIMovement>();

                if (headMovement != null) groundBotHeadMovementInstance = headMovement;
                if (stateMachine != null) groundBotStateInstance = stateMachine;
                if (fieldOfView != null) enemyGroundBotFieldViewInstance = fieldOfView;
                if (navMeshAgent != null) groundBotAI = navMeshAgent;
                if (groundBotAIMovement != null) groundBotAIMovementInstance = groundBotAIMovement;


                enemy.name = "GroundBotGroup2";
                enemy.tag = "GroundBot";
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }

    public void SpawnGroup3()
    {
        for (int i = 0; i < group3.Length; i++)
        {
            if (i < enemyGroupLocations3.Length)
            {
                GameObject enemy = Instantiate(enemyPrefab, enemyGroupLocations3[i].position, enemyGroupLocations3[i].rotation);
                group3[i] = enemy;
                GroundBotHeadMovement headMovement = enemy.GetComponent<GroundBotHeadMovement>();
                GroundBotStateMachine stateMachine = enemy.GetComponent<GroundBotStateMachine>();
                EnemyGroundBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemyGroundBotFieldOfView>();

                if (headMovement != null) groundBotHeadMovementInstance = headMovement;
                if (stateMachine != null) groundBotStateInstance = stateMachine;
                if (fieldOfView != null) enemyGroundBotFieldViewInstance = fieldOfView;

                enemy.name = "GroundBotGroup3";
                enemy.tag = "GroundBot";
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }
    public void SpawnGroup4()
    {
        for (int i = 0; i < group4.Length; i++)
        {
            if (i < enemyGroupLocations4.Length)
            {
                GameObject enemy = Instantiate(enemyPrefab, enemyGroupLocations4[i].position, enemyGroupLocations4[i].rotation);
                group4[i] = enemy;
                GroundBotHeadMovement headMovement = enemy.GetComponent<GroundBotHeadMovement>();
                GroundBotStateMachine stateMachine = enemy.GetComponent<GroundBotStateMachine>();
                EnemyGroundBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemyGroundBotFieldOfView>();
                enemy.AddComponent<NavMeshAgent>();
                NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
                navMeshAgent.baseOffset = .5f;
                navMeshAgent.height = 1.0f;
                enemy.AddComponent<GroundBotAIMovement>();
                GroundBotAIMovement groundBotAIMovement = enemy.GetComponent<GroundBotAIMovement>();


                if (headMovement != null) groundBotHeadMovementInstance = headMovement;
                if (stateMachine != null) groundBotStateInstance = stateMachine;
                if (fieldOfView != null) enemyGroundBotFieldViewInstance = fieldOfView;
                if (navMeshAgent != null) groundBotAI = navMeshAgent;
                if (groundBotAIMovement != null) groundBotAIMovementInstance = groundBotAIMovement;


                enemy.name = "GroundBotGroup4";
                enemy.tag = "GroundBot";
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }

    public GameObject ReturnEnemyOne()
    {
        return group1[0];
    }

    public GroundBotStateMachine ReturnGroundBotStateInstance()
    {
        return this.groundBotStateInstance;
    }

    public GroundBotHeadMovement ReturnBotHeadMovement()
    {
        return this.groundBotHeadMovementInstance;
    }

    public EnemyGroundBotFieldOfView ReturnEnemyFieldOfViewInstance()
    {
        return this.enemyGroundBotFieldViewInstance;
    }

    public NavMeshAgent ReturnThisNavAgent()
    {
        return this.groundBotAI;
    }
}

