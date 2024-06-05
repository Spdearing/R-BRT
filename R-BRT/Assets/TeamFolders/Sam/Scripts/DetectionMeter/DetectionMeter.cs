using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionMeter : MonoBehaviour
{
    [SerializeField] Image detectionMeter;
    [SerializeField] float startingDetection = 0.0f;
    [SerializeField] float detectionIncrement = 0.75f;
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
        startingDetection -= detectionLost;
        startingDetection = Mathf.Clamp(startingDetection, 0, maxDetection);
        detectionMeter.fillAmount = startingDetection / maxDetection;
    }

    public float ReturnStartingDetection()
    {
        return startingDetection;
    }
}
