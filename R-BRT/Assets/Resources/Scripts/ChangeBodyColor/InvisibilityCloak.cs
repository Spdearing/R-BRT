using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvisibilityCloak : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    [Header("Renderer")]
    [SerializeField] private SkinnedMeshRenderer skinMeshRenderer1;
    [SerializeField] private SkinnedMeshRenderer skinMeshRenderer2;

    // Start is called before the first frame update
    void Start()
    {
        skinMeshRenderer1 = GameObject.Find("LeftArm_RightArm5").GetComponent<SkinnedMeshRenderer>();
        skinMeshRenderer2 = GameObject.Find("LeftArm_RightArm5 (Copy)").GetComponent<SkinnedMeshRenderer>();
    }


    public void TurnInvisible()
    {
        skinMeshRenderer1.enabled = false;
        skinMeshRenderer2.enabled = true;
    }

    public void TurnVisible() 
    {
        skinMeshRenderer1.enabled = true;
        skinMeshRenderer2.enabled = false;
    }
}
