using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximity : MonoBehaviour
{
    [SerializeField] GameObject enemyRaycast;
    [SerializeField] GameObject player;
    [SerializeField] bool playerIsSeen;


    private void Start()
    {
        player = GameObject.Find("Player");
        
    }

    private void Update()
    {
        if (playerIsSeen)
        {
            Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            enemyRaycast.transform.LookAt(playerPosition);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerIsSeen = true;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsSeen = false;
        }
    }
}
