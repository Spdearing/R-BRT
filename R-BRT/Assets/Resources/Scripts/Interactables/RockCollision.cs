using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    [SerializeField] bool makingSound;

    private void Start()
    {
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            makingSound = true;
        }
    }

    public bool ReturnSound()
    {
        return this.makingSound;
    }
}
