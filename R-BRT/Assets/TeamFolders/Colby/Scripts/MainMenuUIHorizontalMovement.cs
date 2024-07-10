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
    float uiMoveSpeed = 2;
    public float extraMouseRoom = 400;

    // Start is called before the first frame update
    void Start()
    {
        mouseMaxWidth = 200f;
        startingPosition = transform.position.x;
        startingRotation = transform.rotation.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePosX = Input.mousePosition.x;
        if (((1920f/2f) - mouseMaxWidth) < mousePosX && mousePosX < ((1920f/2) + mouseMaxWidth + extraMouseRoom))
        {
            mousePos = mousePosX;
            
        }
        MoveUIWithMouse();
        
    }

    private void MoveUIWithMouse()
    {
        float adjustedMousePos = (mousePos - ((1920f / 2f) - mouseMaxWidth)) / (((1920f / 2f) + mouseMaxWidth + extraMouseRoom) - ((1920f / 2f) - mouseMaxWidth));
        
        float UIPosX = Mathf.Lerp((startingPosition + offsetAmount), (startingPosition - offsetAmount), adjustedMousePos);
        float UIRotY = Mathf.Lerp((startingRotation + rotationAmount), (startingRotation - rotationAmount), adjustedMousePos);

        //float smoothUIRotY = Mathf.Lerp(transform.rotation.y, UIRotY, Time.deltaTime);
        //Vector3 newRotation = new Vector3(transform.rotation.x, smoothUIRotY, transform.rotation.z);

        transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, UIPosX, (uiMoveSpeed * Time.deltaTime)), 0, 0);
        //transform.localRotation = Quaternion.Euler(newRotation);
        transform.localEulerAngles = new Vector3(transform.rotation.x, UIRotY, transform.rotation.z);
    }
}
