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

    // Crouching sensitivity modifier
    [SerializeField] private float crouchSensitivityModifier = 0.5f;
    private float currentSensX;
    private float currentSensY;

    // Transition speed for sensitivity change
    [SerializeField] private float sensitivityTransitionSpeed = 5.0f;

    // Start is called before the first frame update
    private void Start()
    {
        playerController = GameManager.instance.ReturnPlayerController();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize current sensitivity
        currentSensX = sensX;
        currentSensY = sensY;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!playerController.isCameraLocked)
        {
            float targetSensX = sensX;
            float targetSensY = sensY;

            if (playerController.ReturnCrouchingStatus(true))
            {
                targetSensX *= crouchSensitivityModifier;
                targetSensY *= crouchSensitivityModifier;
            }

            // Smoothly interpolate the sensitivity values
            currentSensX = Mathf.Lerp(currentSensX, targetSensX, Time.deltaTime * sensitivityTransitionSpeed);
            currentSensY = Mathf.Lerp(currentSensY, targetSensY, Time.deltaTime * sensitivityTransitionSpeed);

            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * currentSensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * currentSensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Rotate cam and orientation
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }
}
