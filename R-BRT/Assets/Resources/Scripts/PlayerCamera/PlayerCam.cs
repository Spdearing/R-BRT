using UnityEngine;


public class PlayerCam : MonoBehaviour
{
    [Header("Floats")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;

    [Header("Transform")]
    [SerializeField] private Transform orientation;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;

    

    // Start is called before the first frame update
    private void Start()
    {

        playerController = GameManager.instance.ReturnPlayerController();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!playerController.isCameraLocked)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;


            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate cam and orientation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
