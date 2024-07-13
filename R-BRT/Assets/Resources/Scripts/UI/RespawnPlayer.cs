using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public void RespawnMe()
    {
        GameManager.instance.RespawnPlayer();
    }
}
