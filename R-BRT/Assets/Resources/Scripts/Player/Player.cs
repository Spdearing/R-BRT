using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    [SerializeField] float health;



    public void SetHealth(float value)
    {
        health = value;
    }

    public float ReturnHealth()
    {
        return this.health;
    }

}
