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

    [SerializeField] private GameObject RBRToutlineCamera;
    [SerializeField] private GameObject RBRTinfrontCamera;
    [SerializeField] private GameObject S4MoutlineCamera;
    [SerializeField] private GameObject S4MinfrontCamera;
    [SerializeField] private Animator offsetRBRTAnim;
    [SerializeField] private Animator offsetS4MAnim;

    // Start is called before the first frame update
    void Start()
    {
        RBRToutlineCamera = GameObject.Find("RBRTOutlineCamera");
        RBRTinfrontCamera = GameObject.Find("RBRTInfrontCamera");
        S4MoutlineCamera = GameObject.Find("S4MOutlineCamera");
        S4MinfrontCamera = GameObject.Find("S4MInfrontCamera");
        offsetRBRTAnim = GameObject.Find("RBRTOutlineHolder").GetComponent<Animator>();
        offsetS4MAnim = GameObject.Find("S4MOutlineHolder").GetComponent<Animator>();

        RBRToutlineCamera.SetActive(false);
        RBRTinfrontCamera.SetActive(false);
        S4MoutlineCamera.SetActive(false);
        S4MinfrontCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        MoveCameraWithMouse();
        OnMouseHover();
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
            //Debug.DrawRay(transform.position, transform.forward, Color.red);
            Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.tag == "MainMenuRBRT")
            {
                RBRToutlineCamera.SetActive(true);
                RBRTinfrontCamera.SetActive(true);
                offsetRBRTAnim.SetBool("RBRTHovered", true);
            }

            if (hit.collider.tag == "MainMenuS4M")
            {
                S4MoutlineCamera.SetActive(true);
                S4MinfrontCamera.SetActive(true);
                offsetS4MAnim.SetBool("S4MHovered", true);
            }
            
            if (hit.collider.tag != "MainMenuRBRT" && hit.collider.tag != "MainMenuS4M")
            {
                RBRToutlineCamera.SetActive(false);
                RBRTinfrontCamera.SetActive(false);
                S4MoutlineCamera.SetActive(false);
                S4MinfrontCamera.SetActive(false);
                offsetRBRTAnim.SetBool("RBRTHovered", false);
                offsetS4MAnim.SetBool("S4MHovered", false);
            }
        }
    }
}
