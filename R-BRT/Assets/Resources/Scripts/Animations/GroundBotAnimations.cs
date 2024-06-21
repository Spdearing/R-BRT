using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBotAnimations : MonoBehaviour
{
    [SerializeField] private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MakeSuspicious();
    }

    public void MakeSuspicious()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("IsSuspicious", true);
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            anim.SetBool("IsSuspicious", false);
        }
    }

}
