using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] bool playerSpotted;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "FOV")
        {
            playerSpotted = true;
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "FOV")
        {
            playerSpotted = false;
        }
    }

    public bool ReturnPlayerSpotted()
    {
        return this.playerSpotted;
    }
}
