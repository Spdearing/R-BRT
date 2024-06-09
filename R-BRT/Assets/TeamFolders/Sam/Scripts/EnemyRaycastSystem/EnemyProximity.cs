using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProximity : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject enemyRaycast;
    [SerializeField] GameObject player;

    [Header("Bools")]
    [SerializeField] bool playerWithinRange;

    [Header("Scripts")]
    [SerializeField] GroundBotHeadRaycastDetection groundBotDetection;

    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = gameManager.ReturnPlayer();


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
            this.playerWithinRange = true;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.playerWithinRange = false;
        }
    }

    public bool ReturnPlayerProximity()
    {
        return this.playerWithinRange;
    }
}
