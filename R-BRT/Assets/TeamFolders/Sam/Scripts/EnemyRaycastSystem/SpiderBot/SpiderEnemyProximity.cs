using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemyProximity : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject enemyRaycast;
    [SerializeField] GameObject player;

    [Header("Bools")]
    [SerializeField] bool playerWithinRange;

    [Header("Scripts")]
    [SerializeField] SpiderBotHeadRaycastDetection spiderBotDetection;

    [Header("Game Manager")]
    [SerializeField] GameManager gameManager;

    private float capsuleHeight = 0.96f; // Capsule collider height


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = gameManager.ReturnPlayer();


    }

    private void Update()
    {
        if (playerWithinRange && spiderBotDetection.ReturnPlayerDetected() == false)
        {
            Vector3 playerCenterPosition = player.transform.position + new Vector3(0, capsuleHeight / 2, 0);


            enemyRaycast.transform.LookAt(playerCenterPosition);
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
            spiderBotDetection.SetPlayerDetected(false);
        }
    }

    public bool ReturnPlayerProximity()
    {
        return this.playerWithinRange;
    }
}
