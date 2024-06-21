using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    //[SerializeField] TMP_Text shockWaveActive;
    //[SerializeField] TMP_Text shockWaveCountDown;
    //[SerializeField] int shockWaveAbilityCountDown;


    


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //gameManager.ReturnShockWaveAbilityStatus();
        //UpdateShockWaveAvailablilty();

    }

    ////public void UpdateShockWaveAvailablilty()
    //{
    //    Debug.Log("Inside UpdateShockAbility");
    //    if(gameManager.ReturnShockWaveAbilityStatus() == true)
    //    {
    //        Debug.Log("Shock is true");
    //        //shockWaveActive.text = "Shock Wave: Available";
    //    }
    //    else if (gameManager.ReturnShockWaveAbilityStatus() == false)
    //    {
    //        Debug.Log("Shock is false");
    //        shockWaveAbilityCountDown = 7;
    //        shockWaveActive.text = "Shock Wave: Not Available " + shockWaveCountDown.text;
    //        StartCoroutine(ShockWaveAbilityCoolDown());
    //        ResetCountDown();
    //    }
    //}

    //IEnumerator ShockWaveAbilityCoolDown()
    //{
    //    while (shockWaveAbilityCountDown > 0)
    //    {
    //        shockWaveCountDown.text = shockWaveAbilityCountDown.ToString();
    //        yield return new WaitForSeconds(1.0f);
    //        shockWaveAbilityCountDown -= 1;
            
    //        if(shockWaveAbilityCountDown == 0 )
    //        {
    //            shockWaveCountDown.text = "";
    //        }
    //    }
        
    //}
    //void ResetCountDown()
    //{
    //    shockWaveAbilityCountDown = 7;
    //}
}
