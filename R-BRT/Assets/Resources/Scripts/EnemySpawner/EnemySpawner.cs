using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [Header("EnemyGroups")]
    [SerializeField] GameObject[] enemyGroup1 = new GameObject[3];

    [Header("EnemyGroupLocations")]
    [SerializeField] Transform[] enemyGroupLocations = new Transform[3];


    // Start is called before the first frame update
    void Start()
    {
        SpawnGroup1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnGroup1()
    {
        for (int i = 0; i < enemyGroup1.Length; i++)
        {
            if (i < enemyGroupLocations.Length)
            {
                GameObject enemy = Instantiate(enemyGroup1[i], enemyGroupLocations[i].position, enemyGroupLocations[i].rotation);
            }
            else
            {
                Debug.LogWarning("Index out of bounds for enemyGroupLocations array.");
            }
        }
    }
}

