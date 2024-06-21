using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeBodyColor : MonoBehaviour
{

    [SerializeField] private SkinnedMeshRenderer skinMeshRenderer;
    [SerializeField] private Material[] originalMaterials;
    [SerializeField] private Material red;


    // Start is called before the first frame update
    void Start()
    {
        skinMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        originalMaterials[0] = skinMeshRenderer.materials[0];
        originalMaterials[1] = skinMeshRenderer.materials[1];
        originalMaterials[2] = skinMeshRenderer.materials[2];
        originalMaterials[3] = skinMeshRenderer.materials[3];
        originalMaterials[4] = skinMeshRenderer.materials[4];
        originalMaterials[5] = skinMeshRenderer.materials[5];

    }

    // Update is called once per frame
    void Update()
    {
        ChangeColors();
    }

    public void ChangeColors()
    {
        if (Input.GetKey(KeyCode.F))
        {
            skinMeshRenderer.materials[0] = red;
            skinMeshRenderer.materials[1] = red;
            skinMeshRenderer.materials[2] = red;
            skinMeshRenderer.materials[3] = red;
            skinMeshRenderer.materials[4] = red;
            skinMeshRenderer.materials[5] = red;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            skinMeshRenderer.materials[0] = originalMaterials[0];
            skinMeshRenderer.materials[1] = originalMaterials[1];
            skinMeshRenderer.materials[2] = originalMaterials[2];
            skinMeshRenderer.materials[3] = originalMaterials[3];
            skinMeshRenderer.materials[4] = originalMaterials[4];
            skinMeshRenderer.materials[5] = originalMaterials[5];
        }
    }
}
