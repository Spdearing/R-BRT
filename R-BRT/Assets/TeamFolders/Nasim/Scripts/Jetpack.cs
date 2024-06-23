using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jetpack : MonoBehaviour
    {
        public bool canUseJetpack;
        public float jetpackAcceleration = 15f;

        public float delayBetweenPress = 0.25f;
        public bool pressedFirstTime = false;
        public float lastPressedTime;

        [Range(0f, 1f)]
        public float jetpackDownwardVelocity = 0.75f;

        public float consumeFuelDuration = 1.5f;
        public float refuelDurationGrounded = 2f;
        public float refuelDurationAirborne = 3f;
        public float refuelDelay = 1f;

        public float currentFillRatio;
        float lastTimeOfUse;

        PlayerController playerCharacterController;
        [SerializeField] private TrailRenderer tr;

        [SerializeField]
        private Slider fuelMeter;

        void Start()
        {
            playerCharacterController = GetComponent<PlayerController>();
            tr = GetComponent<TrailRenderer>();

            currentFillRatio = 1f;
            tr.emitting = false;
        }

        void Update()
        {
            if (playerCharacterController.ReturnIsGrounded()) 
            {
                canUseJetpack = false;
            } 
            else if (Input.GetKey(KeyCode.Space)) 
            {
                canUseJetpack = true;
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(pressedFirstTime)
                {
                    bool isDoubledPress = Time.time -lastPressedTime <= delayBetweenPress;

                    if(isDoubledPress)
                    {
                        Debug.Log("See if Jetpack Activated");
                        pressedFirstTime = false;
                    }
                }

                else
                {
                    pressedFirstTime = true;
                }

                lastPressedTime = Time.time;
            }

            if(pressedFirstTime && Time.time - lastPressedTime > delayBetweenPress)
            {
                Debug.Log("Jetpack On");
                pressedFirstTime = false;
            }


            bool jetpackIsInUse = canUseJetpack && currentFillRatio > 0f && Input.GetKey(KeyCode.Space);

            if(jetpackIsInUse) {

                tr.emitting = true;

                lastTimeOfUse = Time.time;

                float totalAcceleration = jetpackAcceleration;

                totalAcceleration += playerCharacterController.ReturnGravity();

                if (playerCharacterController.rb.velocity.y < 0f) {

                    totalAcceleration += ((-playerCharacterController.rb.velocity.y / Time.deltaTime) * jetpackDownwardVelocity);
                }

                playerCharacterController.rb.velocity += Vector3.up * totalAcceleration * Time.deltaTime;

                currentFillRatio = currentFillRatio - (Time.deltaTime / consumeFuelDuration);

            } else {
                tr.emitting = false;

                if (Time.time - lastTimeOfUse >= refuelDelay)
                {
                    float refillRate = 1 / (playerCharacterController.ReturnIsGrounded() ? refuelDurationGrounded : refuelDurationAirborne);
                    currentFillRatio = currentFillRatio + Time.deltaTime * refillRate;

                }
                currentFillRatio = Mathf.Clamp01(currentFillRatio);
            }

            fuelMeter.value = currentFillRatio;

        }
    }
