using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingBotSpawner : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Int")]
    [SerializeField] private int enemiesSpawned;

    [Header("EnemyGroups")]
    [SerializeField] private GameObject[] group1;
    [SerializeField] private GameObject[] group2;

    [Header("EnemyGroupLocations")]
    [SerializeField] Transform[] enemyGroupLocations1;
    [SerializeField] Transform[] enemyGroupLocations2;

    [Header("References")]
    [SerializeField] FlyingBotStateMachine flyingBotStateInstance;
    [SerializeField] EnemyFlyingBotFieldOfView enemyFlyingBotFieldOfViewInstance;

    void Start()
    {
        enemyPrefab = Resources.Load<GameObject>("Sam's_Prefabs/FlyingBotFinal");
        group1 = new GameObject[4];
        group2 = new GameObject[1];
        enemiesSpawned = 0;
        SpawnGroup1();
        SpawnGroup2();
        ToggleGroup2();
    }

    void SpawnGroup1()
    {
        for (int i = 0; i < group1.Length; i++)
        {
            if (i < enemyGroupLocations1.Length)
            {

                GameObject enemy = Instantiate(enemyPrefab, enemyGroupLocations1[i].position, enemyGroupLocations1[i].rotation);
                group1[i] = enemy;
               
                FlyingBotStateMachine stateMachine = enemy.GetComponent<FlyingBotStateMachine>();
                EnemyFlyingBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemyFlyingBotFieldOfView>();


                if (stateMachine != null) flyingBotStateInstance = stateMachine;
                if (fieldOfView != null) enemyFlyingBotFieldOfViewInstance = fieldOfView;

                enemy.name = "FlyingBotGroup1Lobby" + (enemiesSpawned + 1).ToString();
                enemiesSpawned++;
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

                FlyingBotStateMachine stateMachine = enemy.GetComponent<FlyingBotStateMachine>();
                EnemyFlyingBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemyFlyingBotFieldOfView>();


                if (stateMachine != null) flyingBotStateInstance = stateMachine;
                if (fieldOfView != null) enemyFlyingBotFieldOfViewInstance = fieldOfView;

                enemy.name = "FlyingBotGroup2";
                
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }

    public void ToggleGroup1()
    {
        for (int i = 0; i < group1.Length; i++)
        {
            bool isActive = group1[i].activeSelf;
            group1[i].SetActive(!isActive);
        }
    }

    public FlyingBotStateMachine ReturnFlyingBotStateInstance()
    {
        return this.flyingBotStateInstance;
    }

    public EnemyFlyingBotFieldOfView ReturnEnemyFieldOfViewInstance()
    {
        return this.enemyFlyingBotFieldOfViewInstance;
    }

    public void ToggleGroup2()
    {
        bool isActive = group2[0].activeSelf;
        group2[0].SetActive(!isActive);
    }
}

