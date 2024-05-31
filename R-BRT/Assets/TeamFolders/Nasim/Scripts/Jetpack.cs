using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    public bool canUseJetpack;
    public float jetpackAcceleration = 25f;
    
    [Range(0f, 1f)]
    public float jetpackDownwardVelocityCanceling = 0.75f;

    public float fuelConsumedDuration = 1.5f;
    public float refuelDurationGrounded = 2f;
    public float refuelDurationFlying = 5f;
    public float refuelDelay = 1f;

    public float currentFuelRatio;
    float lastTimeofUse;

    PlayerMovement3d playerCharacterController;
    [SerializeField] private TrailRenderer tr;


    void Start()
    {
        playerCharacterController = GetComponent<PlayerMovement3d>();

        currentFuelRatio = 1f;
        tr.emitting = false;

    }


    void Update()
    {
        if (playerCharacterController.isGrounded)
        {
            canUseJetpack = false;
        }
        else if (!playerCharacterController.hasJumpedThisFrame && Input.GetKeyDown(KeyCode.Space))
        {
            canUseJetpack = true;
        }

        bool jetpackIsInUse = canUseJetpack && currentFuelRatio > 0f && Input.GetKey(KeyCode.Space);

        if(jetpackIsInUse)
        {
            tr.emitting = true;

            lastTimeofUse = Time.time;

            float totalAcceleration = jetpackAcceleration;

            totalAcceleration += playerCharacterController.gravity;

            if (playerCharacterController.velocity.y < 0f)
            {
                totalAcceleration += ((-playerCharacterController.velocity.y / Time.deltaTime) * jetpackDownwardVelocityCanceling);
            }

            playerCharacterController.velocity += Vector3.up * totalAcceleration * Time.deltaTime;

            currentFuelRatio = currentFuelRatio - (Time.deltaTime / consumeDuration);
        }

        else 
        {
            tr.emitting = false;

            if (Time.time - lastTimeofUse >= refuelDelay)
            {
                float refuelRate = 1 / (playerCharacterController.isGrounded ? refuelDurationGrounded : refuelDurationFlying);
                currentFuelRatio = currentFuelRatio + Time.deltaTime * refuelRate;

            }
            currentFuelRatio = Mathf.Clamp01(currentFuelRatio);
        }
    }
}
