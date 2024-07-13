using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{

    Vector3 mousePos;
    float currentMousePos;
    float maxMousePos = 1920.0f;
    float minCamPosX = 73.22f;
    float maxCamPosX = 73.5f;
    float minCamPosZ = 184.86f;
    float maxCamPosZ = 185.32f;
    float cameraSpeed = 3f;

    [SerializeField] private GameObject outlineCamera;
    [SerializeField] private GameObject infrontCamera;
    [SerializeField] private Animator offsetRBRT;
    [SerializeField] private Animator offsetS4M;

    // Start is called before the first frame update
    void Start()
    {
        outlineCamera = GameObject.Find("OutlineCamera");
        infrontCamera = GameObject.Find("InfrontCamera");
        offsetRBRT = GameObject.Find("RBRTOutlineHolder").GetComponent<Animator>();
        offsetS4M = GameObject.Find("S4MOutlineHolder").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        MoveCameraWithMouse();
    }

    private void MoveCameraWithMouse()
    {
        currentMousePos = mousePos.x;
        float mousePosPercent;
        mousePosPercent = currentMousePos / maxMousePos;
        float CamPosX = Mathf.Lerp(minCamPosX, maxCamPosX, mousePosPercent);
        float CamPosZ = Mathf.Lerp(maxCamPosZ, minCamPosZ, mousePosPercent);
        transform.localPosition = new Vector3(Mathf.Lerp(transform.position.x, CamPosX, (cameraSpeed * Time.deltaTime)), transform.position.y, Mathf.Lerp(transform.position.z, CamPosZ, (cameraSpeed * Time.deltaTime)));
    }

    private void OnMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.tag == "") ;
        }
    }
}
