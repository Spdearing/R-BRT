using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionMeter : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] Image detectionMeter; //Image that will be filled, or depleted because of the script.

    [Header("Floats")]
    [SerializeField] float startingDetection = 0.0f;
    [SerializeField] float detectionIncrement = 0.5f;
    [SerializeField] float maxDetection = 200.0f;

    void Start()
    {
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
}
