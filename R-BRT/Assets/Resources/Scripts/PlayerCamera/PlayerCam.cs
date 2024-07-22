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

    // Smoothing variables
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;
    [SerializeField] private float mouseSmoothTime = 0.03f;

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

            // Get raw mouse input
            Vector2 targetMouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            // Smooth the mouse input
            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

            float mouseX = currentMouseDelta.x * Time.deltaTime * currentSensX;
            float mouseY = currentMouseDelta.y * Time.deltaTime * currentSensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Rotate cam and orientation
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }
}
