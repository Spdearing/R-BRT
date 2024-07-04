using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvisibilityCloak : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private SkinnedMeshRenderer skinMeshRenderer1;
    [SerializeField] private SkinnedMeshRenderer skinMeshRenderer2;

    // Start is called before the first frame update
    void Start()
    {
        skinMeshRenderer1 = GameManager.instance.ReturnRendererOne();
        skinMeshRenderer2 = GameManager.instance.ReturnRendererTwo();
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
