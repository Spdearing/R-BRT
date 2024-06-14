using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] bool playerSpotted;

    [Header("Scripts")]
    [SerializeField] private PlayerDetectionState playerDetectionState;

    private void Update()
    {
        DetectingPlayer();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerSpotted = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerSpotted = false;
        }
    }

    public bool ReturnPlayerSpotted()
    {
        return this.playerSpotted;
    }

    void DetectingPlayer()
    {
        playerDetectionState.ChangeDetectionState(PlayerDetectionState.DetectionState.beingDetected);
    }


}
