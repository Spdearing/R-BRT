using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
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

    [Header("Nav Mesh Agent")]
    [SerializeField] private NavMeshAgent groundBotAI;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyPrefab = Resources.Load<GameObject>("Sam's_Prefabs/groundBotDone");
        group1 = new GameObject[1];
        group2 = new GameObject[4];
        group3 = new GameObject[6];
        group3 = new GameObject[3];
        SpawnGroup1();
        SpawnGroup2();
        SpawnGroup3();
    }

    void SpawnGroup1()
    {
        for (int i = 0; i < group1.Length; i++)
        {
            if (i < enemyGroupLocations1.Length)
            {
                Debug.Log(enemyGroupLocations1[i]);
                Debug.Log(enemyPrefab);
                Debug.Log(enemyGroupLocations1[i].rotation);

                GameObject enemy = Instantiate(enemyPrefab, enemyGroupLocations1[i].position, enemyGroupLocations1[i].rotation);
                group1[i] = enemy;
                GroundBotHeadMovement headMovement = enemy.GetComponent<GroundBotHeadMovement>();
                GroundBotStateMachine stateMachine = enemy.GetComponent<GroundBotStateMachine>();
                EnemyGroundBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemyGroundBotFieldOfView>();


                if (headMovement != null) groundBotHeadMovementInstance = headMovement;
                if (stateMachine != null) groundBotStateInstance = stateMachine;
                if (fieldOfView != null) enemyGroundBotFieldViewInstance = fieldOfView;

                enemy.name = "GroundBotGroup1";
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }

    void SpawnGroup2()
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


                if (headMovement != null) groundBotHeadMovementInstance = headMovement;
                if (stateMachine != null) groundBotStateInstance = stateMachine;
                if (fieldOfView != null) enemyGroundBotFieldViewInstance = fieldOfView;

                enemy.name = "GroundBotGroup2";
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }

    void SpawnGroup3()
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
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
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

