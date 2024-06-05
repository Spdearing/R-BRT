using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionMeter : MonoBehaviour
{

    [SerializeField] Image detectionMeter;
    [SerializeField] float startingDetection;

    // Start is called before the first frame update
    void Start()
    {
        startingDetection = 0.0f;
        detectionMeter.fillAmount = startingDetection;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void IncreaseDetection(float detection)
    {
        startingDetection += detection;
        detectionMeter.fillAmount = startingDetection / 200.0f;
    }

    public void DecreaseDetection(float detectionLost)
    {
        startingDetection -= detectionLost;
        startingDetection = Mathf.Clamp(startingDetection, 0, 200);

        detectionMeter.fillAmount = startingDetection / 200.0f;
    }

    public float ReturnStartingDetection()
    {
        return this.startingDetection;
    }
}
