using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionMeter : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] Image detectionMeter; //Image that will be filled, or depleted because of the script.

    [Header("Floats")]
    [SerializeField] float startingDetection;
    [SerializeField] float detectionIncrement;
    [SerializeField] float maxDetection;

    void Start()
    {
        Debug.Log("DetectionMeter is popping");
        startingDetection = 0.0f;
        detectionIncrement = .75f;
        maxDetection = 201.0f;
        detectionMeter.fillAmount = startingDetection / maxDetection;
    }

    public void IncreaseDetection(float detection)
    {
        startingDetection += detection * Time.deltaTime * detectionIncrement;
        startingDetection = Mathf.Clamp(startingDetection, 0, maxDetection);
        detectionMeter.fillAmount = startingDetection / maxDetection;
    }

    public void DecreaseDetection(float detectionLost)
    {
        startingDetection -= detectionLost * Time.deltaTime * detectionIncrement;
        startingDetection = Mathf.Clamp(startingDetection, 0, maxDetection);
        detectionMeter.fillAmount = startingDetection / maxDetection;
    }

    public float ReturnStartingDetection()
    {
        return startingDetection;
    }


    public void SetDetectionAmount(float detectionAmount)
    {
        startingDetection = detectionAmount;
    }

    public float GetDetectionMax()
    {
        return this.maxDetection;
    }
}
