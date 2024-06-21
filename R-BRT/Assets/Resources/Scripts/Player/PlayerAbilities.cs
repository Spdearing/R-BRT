using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]  GameManager gameManager;
    [SerializeField] HUD playerHud;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //UseShockWaveAbility();
    }


    //public void UseShockWaveAbility()
    //{
    //    if(Input.GetKeyDown(KeyCode.E) && gameManager.ReturnShockWaveAbilityStatus() == true) 
    //    { 
    //        StartCoroutine(ShockWaveAvailabilityCoolDown());
    //    }
    //}

    //public IEnumerator ShockWaveAvailabilityCoolDown()
    //{
    //    gameManager.SetShockWaveAbilityStatus(false);
    //    playerHud.UpdateShockWaveAvailablilty();
    //    yield return new WaitForSeconds(7.0f);
    //    gameManager.SetShockWaveAbilityStatus(true);
    //    playerHud.UpdateShockWaveAvailablilty();
    //}
}
