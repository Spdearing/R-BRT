using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerDetectionState playerDetectionState;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        playerDetectionState = GameObject.Find("Player").GetComponent<PlayerDetectionState>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        DetectingPlayer();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gameManager.SetPlayerIsSpotted(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManager.SetPlayerIsSpotted(false);
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.meterRepleneshing);
        }
    }


    void DetectingPlayer()
    {
        if (gameManager.ReturnPlayerSpotted() == true)
        {
            playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);
            gameManager.SetPlayerIsSpotted(false);
        }
    }
}
