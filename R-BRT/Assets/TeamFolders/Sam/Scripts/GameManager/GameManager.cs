using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool shockwaveAbilityActive;



    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        shockwaveAbilityActive = true;

    }

    public bool ReturnShockWaveAbilityStatus()
    {
        return shockwaveAbilityActive;
    }

    public void SetShockWaveAbilityStatus(bool value)
    {
        shockwaveAbilityActive = value;
    }    
}
