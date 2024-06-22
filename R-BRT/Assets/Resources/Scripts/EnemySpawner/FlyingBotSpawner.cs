using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class FlyingBotSpawner : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;

    [Header("EnemyGroups")]
    [SerializeField] GameObject[] group1 = new GameObject[3];

    [Header("EnemyGroupLocations")]
    [SerializeField] Transform[] enemyGroupLocations = new Transform[3];

    [Header("References")]
    [SerializeField] FlyingBotStateMachine flyingBotStateInstance;
    [SerializeField] EnemyFlyingBotFieldOfView enemyFlyingBotFieldViewInstance;

    // Start is called before the first frame update
    void Start()
    {
        enemyPrefab = Resources.Load<GameObject>("Sam's_Prefabs/FlyingBotFinal");
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
               
                FlyingBotStateMachine stateMachine = enemy.GetComponent<FlyingBotStateMachine>();
                EnemyFlyingBotFieldOfView fieldOfView = enemy.GetComponentInChildren<EnemyFlyingBotFieldOfView>();


                if (stateMachine != null) flyingBotStateInstance = stateMachine;
                if (fieldOfView != null) enemyFlyingBotFieldViewInstance = fieldOfView;

                enemy.name = "FlyingBotGroup1";
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }

    public FlyingBotStateMachine ReturnFlyingBotStateInstance()
    {
        return this.flyingBotStateInstance;
    }

    public EnemyFlyingBotFieldOfView ReturnEnemyFieldOfViewInstance()
    {
        return this.enemyFlyingBotFieldViewInstance;
    }
}

