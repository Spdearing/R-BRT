using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] RockCollision rockCollision;
    [SerializeField] PickUpObject rock;

    [SerializeField] bool hasJetPack;




    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        rockCollision = GameObject.Find("Rocks").GetComponent<RockCollision>();
        rock = GetComponent<PickUpObject>();
    }

    public void SendOutNoise()
    {
        if (rockCollision.ReturnSound() == true)
        {
            GameObject throwable = rock.ReturnThisObject();

            
            AllDirectionRaycast raycastComponent = throwable.GetComponent<AllDirectionRaycast>();

            
            if (raycastComponent != null)
            {
                raycastComponent.enabled = true;
            }
            else
            {
                Debug.LogWarning("AllDirectionRaycast component not found on the throwable object.");
            }
        }
    }

    public void SetJetPackStatus(bool value)
    {
        hasJetPack = value;
    }

    public bool CanUseJetPack()
    {
        return this.hasJetPack;
    }
}
