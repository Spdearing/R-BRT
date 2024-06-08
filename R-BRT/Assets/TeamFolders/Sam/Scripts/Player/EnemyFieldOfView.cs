using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    [SerializeField] bool playerSpotted;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Seeing the Player");
            Debug.Log(playerSpotted);
            playerSpotted = true;
            Debug.Log(playerSpotted);
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player went away");
            playerSpotted = false;
        }
    }

    public bool ReturnPlayerSpotted()
    {
        return this.playerSpotted;
    }
}
