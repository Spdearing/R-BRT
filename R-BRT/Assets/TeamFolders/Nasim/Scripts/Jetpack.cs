using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jetpack : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] private bool pressedFirstTime;
    [SerializeField] private bool canUseJetpack;
    [SerializeField] private bool isUsingJetpack;

    [Header("Floats")]
    [SerializeField] private float jetpackAcceleration = 15f;
    [SerializeField] private float delayBetweenPress = 0.25f;
    [SerializeField] private float lastPressedTime;
    [SerializeField] private float jetpackDownwardVelocity = 0.75f;
    [SerializeField] private float consumeFuelDuration = 1.5f;
    [SerializeField] private float refuelDurationGrounded = 2f;
    [SerializeField] private float refuelDurationAirborne = 3f;
    [SerializeField] private float refuelDelay = 1f;
    [SerializeField] private float currentFillRatio;
    [SerializeField] private float lastTimeOfUse;

    [Range(0f, 1f)]


    [Header("Scripts")]
    [SerializeField] private PlayerController playerCharacterController;


    [Header("Images")]
    [SerializeField] private Image fuelMeter;

    void Start()
    {

        pressedFirstTime = false;
        playerCharacterController = GameManager.instance.ReturnPlayerController();
        fuelMeter = GameManager.instance.ReturnFuelMeterSlider();


        currentFillRatio = 1f;

    }

    void Update()
    {
        if (playerCharacterController.ReturnIsGrounded())
        {
            canUseJetpack = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            canUseJetpack = true;
            pressedFirstTime = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pressedFirstTime)
            {
                bool isDoubledPress = Time.time - lastPressedTime <= delayBetweenPress;

                if (isDoubledPress)
                {
                    
                    pressedFirstTime = false;
                }
            }

            lastPressedTime = Time.time;
        }

        if (pressedFirstTime && Time.time - lastPressedTime > delayBetweenPress)
        {
            
            pressedFirstTime = false;
        }


        bool jetpackIsInUse = canUseJetpack && currentFillRatio > 0f && Input.GetKey(KeyCode.Space);

        if (jetpackIsInUse)
        {
            isUsingJetpack = true;
 

            lastTimeOfUse = Time.time;

            float totalAcceleration = jetpackAcceleration;

            totalAcceleration += playerCharacterController.ReturnGravity();

            if (playerCharacterController.rb.velocity.y < 0f)
            {

                totalAcceleration += ((-playerCharacterController.rb.velocity.y / Time.deltaTime) * jetpackDownwardVelocity);
            }

            playerCharacterController.rb.velocity += Vector3.up * totalAcceleration * Time.deltaTime;

            currentFillRatio = currentFillRatio - (Time.deltaTime / consumeFuelDuration);

        }
        else
        {
            isUsingJetpack = false;


            if (Time.time - lastTimeOfUse >= refuelDelay)
            {
                float refillRate = 1 / (playerCharacterController.ReturnIsGrounded() ? refuelDurationGrounded : refuelDurationAirborne);
                currentFillRatio = currentFillRatio + Time.deltaTime * refillRate;

            }
            currentFillRatio = Mathf.Clamp01(currentFillRatio);
        }

        fuelMeter.fillAmount = currentFillRatio;

    }

    public bool IsUsingJetpack(bool value)
    {
        return this.isUsingJetpack;
    }
}
