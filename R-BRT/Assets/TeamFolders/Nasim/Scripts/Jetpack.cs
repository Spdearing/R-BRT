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
    [SerializeField] private bool doublePressSpace;

    [Header("Floats")]
    [SerializeField] private float jetpackAcceleration = 15f;
    [SerializeField] private float delayBetweenPress = 0.50f;
    [SerializeField] private float lastPressedTime;
    [SerializeField] private float jetpackDownwardVelocity = 0.75f;
    [SerializeField] private float consumeFuelDuration = 1.5f;
    [SerializeField] private float refuelDurationGrounded = 2f;
    [SerializeField] private float refuelDurationAirborne = 3f;
    [SerializeField] private float refuelDelay = 1f;
    [SerializeField] private float currentFillRatio;
    [SerializeField] private float lastTimeOfUse;

    [Header("Audio Source")]
    [SerializeField] private AudioSource jetpackSound;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerCharacterController;

    [Header("Images")]
    [SerializeField] private Image fuelMeter;

    void Start()
    {
        pressedFirstTime = false;
        doublePressSpace = false;
        playerCharacterController = GameManager.instance.ReturnPlayerController();
        fuelMeter = GameManager.instance.ReturnFuelMeterSlider();
        currentFillRatio = 1f;
        jetpackSound = GameManager.instance.ReturnJetpackSound();
    }

    void Update()
    {
        HandleGroundCheck();
        HandleInput();
        HandleJetpackUsage();
        UpdateFuelMeter();
    }

    private void HandleGroundCheck()
    {
        canUseJetpack = !playerCharacterController.ReturnIsGrounded();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pressedFirstTime && Time.time - lastPressedTime <= delayBetweenPress)
            {
                doublePressSpace = true;
                pressedFirstTime = false;
            }
            else
            {
                pressedFirstTime = true;
                doublePressSpace = false;
            }

            lastPressedTime = Time.time;
        }

        if (pressedFirstTime && Time.time - lastPressedTime > delayBetweenPress)
        {
            pressedFirstTime = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            doublePressSpace = false;
        }
    }

    private void HandleJetpackUsage()
    {
        bool jetpackIsInUse = canUseJetpack && currentFillRatio > 0f && doublePressSpace;

        if (jetpackIsInUse)
        {
            StartJetpack();
        }
        else
        {
            StopJetpack();
            RefuelJetpack();
        }
    }

    private void StartJetpack()
    {
        isUsingJetpack = true;
        lastTimeOfUse = Time.time;
        float totalAcceleration = jetpackAcceleration + playerCharacterController.ReturnGravity();

        if (playerCharacterController.rb.velocity.y < 0f)
        {
            totalAcceleration += ((-playerCharacterController.rb.velocity.y / Time.deltaTime) * jetpackDownwardVelocity);
        }

        playerCharacterController.rb.velocity += Vector3.up * totalAcceleration * Time.deltaTime;
        currentFillRatio -= Time.deltaTime / consumeFuelDuration;
        currentFillRatio = Mathf.Clamp01(currentFillRatio);

        // Ensure the jetpack sound only starts if it is not already playing
        if (!jetpackSound.isPlaying)
        {
            jetpackSound.Play();
        }
    }

    private void StopJetpack()
    {
        isUsingJetpack = false;

        // Ensure the jetpack sound stops when the jetpack is not in use
        if (jetpackSound.isPlaying)
        {
            jetpackSound.Stop();
        }
    }

    private void RefuelJetpack()
    {
        if (Time.time - lastTimeOfUse >= refuelDelay)
        {
            float refillRate = 1 / (playerCharacterController.ReturnIsGrounded() ? refuelDurationGrounded : refuelDurationAirborne);
            currentFillRatio += Time.deltaTime * refillRate;
            currentFillRatio = Mathf.Clamp01(currentFillRatio);
        }
    }

    private void UpdateFuelMeter()
    {
        fuelMeter.fillAmount = currentFillRatio;
    }

    public bool IsUsingJetpack()
    {
        return this.isUsingJetpack;
    }
}
