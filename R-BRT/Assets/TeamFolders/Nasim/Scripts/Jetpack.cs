using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
    {
        public bool canUseJetpack;
        public float jetpackAcceleration = 25f;

        [Range(0f, 1f)]
        public float jetpackDownwardVelocity = 0.75f;

        public float consumeFuelDuration = 1.5f;
        public float refuelDurationGrounded = 2f;
        public float refuelDurationAirborne = 5f;
        public float refuelDelay = 1f;

        public float currentFillRatio;
        float lastTimeOfUse;

        PlayerController playerCharacterController;
        [SerializeField] private TrailRenderer tr;

        void Start()
        {
            playerCharacterController = GetComponent<PlayerController>();

            currentFillRatio = 1f;
            tr.emitting = false;
        }

        void Update()
        {
            if (playerCharacterController.isGrounded) {
                canUseJetpack = false;
            } else if (!playerCharacterController.hasJumpedThisFrame && Input.GetKeyDown(KeyCode.Space)) {
                canUseJetpack = true;
            }


            bool jetpackIsInUse = canUseJetpack && currentFillRatio > 0f && Input.GetKey(KeyCode.Space);

            if(jetpackIsInUse) {

                tr.emitting = true;

                lastTimeOfUse = Time.time;

                float totalAcceleration = jetpackAcceleration;

                totalAcceleration += playerCharacterController.gravity;

                if (playerCharacterController.velocity.y < 0f) {

                    totalAcceleration += ((-playerCharacterController.velocity.y / Time.deltaTime) * jetpackDownwardVelocity);
                }

                playerCharacterController.velocity += Vector3.up * totalAcceleration * Time.deltaTime;

                currentFillRatio = currentFillRatio - (Time.deltaTime / consumeFuelDuration);

            } else {
                tr.emitting = false;

                if (Time.time - lastTimeOfUse >= refuelDelay)
                {
                    float refillRate = 1 / (playerCharacterController.isGrounded ? refuelDurationGrounded : refuelDurationAirborne);
                    currentFillRatio = currentFillRatio + Time.deltaTime * refillRate;

                }
                currentFillRatio = Mathf.Clamp01(currentFillRatio);
            }

             

        }
    }
