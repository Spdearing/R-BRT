using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderBotSpawner : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;

    [Header("EnemyGroups")]
    [SerializeField] GameObject[] group1 = new GameObject[1];
    [SerializeField] GameObject[] group2 = new GameObject[4];

    [Header("EnemyGroupLocations")]
    [SerializeField] Transform[] enemyGroup1Locations = new Transform[1];
    [SerializeField] Transform[] enemyGroup2Locations = new Transform[4];

    [Header("References")]
    [SerializeField] SpiderBotStateMachine spiderBotStateInstance;
    [SerializeField] EnemySpiderBotFieldOfView enemySpiderBotFieldOfViewInstance;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SpiderBotSpawner popping");
        enemyPrefab = Resources.Load<GameObject>("Sam's_Prefabs/CameraBotFinal");
        SpawnGroup1();
    }

    void SpawnGroup1()
    {
        for (int i = 0; i < group1.Length; i++)
        {
            if (i < enemyGroup1Locations.Length)
            {

                GameObject enemy = Instantiate(enemyPrefab, enemyGroup1Locations[i].position, enemyGroup1Locations[i].rotation);
                group1[i] = enemy;
                SpiderBotStateMachine stateMachine = enemy.GetComponent<SpiderBotStateMachine>();
                EnemySpiderBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemySpiderBotFieldOfView>();

                if (stateMachine != null) spiderBotStateInstance = stateMachine;
                if (fieldOfView != null) enemySpiderBotFieldOfViewInstance = fieldOfView;

                enemy.name = "SpiderBotGroup1";
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
            if (i < enemyGroup2Locations.Length)
            {

                GameObject enemy = Instantiate(enemyPrefab, enemyGroup2Locations[i].position, enemyGroup2Locations[i].rotation);
                group2[i] = enemy;
                SpiderBotStateMachine stateMachine = enemy.GetComponent<SpiderBotStateMachine>();
                EnemySpiderBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemySpiderBotFieldOfView>();

                if (stateMachine != null) spiderBotStateInstance = stateMachine;
                if (fieldOfView != null) enemySpiderBotFieldOfViewInstance = fieldOfView;

                enemy.name = "SpiderBotGroup2";
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }

    public SpiderBotStateMachine ReturnSpiderBotStateInstance()
    {
        return this.spiderBotStateInstance;
    }

    public EnemySpiderBotFieldOfView ReturnEnemySpiderBotFieldOfViewInstance()
    {
        return this.enemySpiderBotFieldOfViewInstance;
    }
}

