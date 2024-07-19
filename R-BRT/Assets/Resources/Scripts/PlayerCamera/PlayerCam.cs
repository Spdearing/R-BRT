using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Floats")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    private float xRotation = 0f;
    private float yRotation = 0f;

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
            sensX = 50.0f;
            sensY = 50.0f;

            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Rotate cam and orientation
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }
}
