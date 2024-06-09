using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    [SerializeField] bool makingSound;
    [SerializeField] GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            makingSound = true;
            gameManager.SendOutNoise();
        }
    }

    public bool ReturnSound()
    {
        return this.makingSound;
    }
}
