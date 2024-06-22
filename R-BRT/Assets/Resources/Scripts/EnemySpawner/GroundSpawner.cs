using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class GroundBotSpawner : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;

    [Header("EnemyGroups")]
    [SerializeField] GameObject[] group1 = new GameObject[3];

    [Header("EnemyGroupLocations")]
    [SerializeField] Transform[] enemyGroupLocations = new Transform[3];

    [Header("References")]
    [SerializeField] GroundBotStateMachine groundBotStateInstance;
    [SerializeField] GroundBotHeadMovement groundBotHeadMovementInstance;
    [SerializeField] EnemyGroundBotFieldOfView enemyGroundBotFieldViewInstance;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyPrefab = Resources.Load<GameObject>("Sam's_Prefabs/groundBotDone");
        SpawnGroup1();
    }

    void SpawnGroup1()
    {
        for (int i = 0; i < group1.Length; i++)
        {
            if (i < enemyGroupLocations.Length)
            {
                
                GameObject enemy = Instantiate(enemyPrefab, enemyGroupLocations[i].position, enemyGroupLocations[i].rotation);
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
}

