using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    Rigidbody rb;
    public GameObject player;
    Jetpack jetPack;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Battery";
        jetPack = player.GetComponent<Jetpack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        jetPack.enabled = true;
        Destroy(gameObject);
    }
}
