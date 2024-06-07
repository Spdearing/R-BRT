using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximity : MonoBehaviour
{
    [SerializeField] GameObject enemyRaycast;
    [SerializeField] GameObject player;
    [SerializeField] bool playerWithinRange;

    [SerializeField] GroundBotHeadRaycastDetection groundBotDetection;


    private void Start()
    {
        player = GameObject.Find("Player");
        
    }

    private void Update()
    {
        if (playerWithinRange && groundBotDetection.ReturnPlayerDetected() == false)
        {
            Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            enemyRaycast.transform.LookAt(playerPosition);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerWithinRange = true;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerWithinRange = false;
        }
    }

    public bool ReturnPlayerProximity()
    {
        return this.playerWithinRange;
    }
}
