using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : MonoBehaviour
{
    public bool invisibleTimer;
    public bool invisible = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void invisibilityActive()
    {
        gameObject.tag = "Invisible";
    }

}
