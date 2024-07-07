using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIHorizontalMovement : MonoBehaviour
{

    float mousePosX;
    float mousePos;
    float startingPosition;
    float startingRotation;
    public float rotationAmount;
    public float offsetAmount;
    public float mouseMaxWidth;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position.x;
        startingRotation = transform.rotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosX = Input.mousePosition.x;
        if (((1920f/2f) - mouseMaxWidth) < mousePosX && mousePosX < ((1920f/2) + mouseMaxWidth))
        {
            mousePos = mousePosX;
        }
    }

    private void MoveUIWithMouse()
    {
        float adjustedMousePos = mousePos / (((1920f / 2f) + mouseMaxWidth) - ((1920f / 2f) - mouseMaxWidth));
        
        float UIPosX = Mathf.Lerp((startingPosition - offsetAmount), (startingPosition + offsetAmount), adjustedMousePos);
        //float UIRotY = Mathf.Lerp(maxCamPosZ, minCamPosZ, mousePosPercent);
    }
}
