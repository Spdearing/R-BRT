using System.Collections;
using UnityEngine;

public class GroundBotHeadMovement : MonoBehaviour
{
    [SerializeField] private bool robotIsActive = true;
    [SerializeField] private bool playerIsSpotted;
    [SerializeField] private bool rotatingLeft = true;
    [SerializeField] private bool isPaused = false;

    [SerializeField] private float rotationAngle = 45f; // Rotation angle in degrees
    [SerializeField] private float rotationSpeed = 25f; // Rotation speed
    [SerializeField] private GameObject groundBotHead;
    [SerializeField] private Renderer groundBotHeadColor;
    [SerializeField] private EnemyFieldOfView enemyFieldOfView;
    [SerializeField] private Material green;
    [SerializeField] private Material red;

    private float startYRotation;
    private float targetYRotation;

    void Start()
    {
        // Ensure references within the prefab instance
        enemyFieldOfView = GetComponentInChildren<EnemyFieldOfView>();
        groundBotHead = transform.Find("GroundBotHead").gameObject;
        groundBotHeadColor = groundBotHead.GetComponent<Renderer>();

        // Set initial material
        groundBotHeadColor.material = green;

        startYRotation = transform.eulerAngles.y;
        SetTargetYRotation();
    }

    void Update()
    {
        if (robotIsActive && !isPaused && !playerIsSpotted)
        {
            if (!enemyFieldOfView.ReturnPlayerSpotted())
            {
                RotateHead();
            }
        }

        // Check for player detection and update color accordingly
        if (playerIsSpotted)
        {
            groundBotHeadColor.material = red;
        }
        else
        {
            groundBotHeadColor.material = green;
        }
    }

    private void RotateHead()
    {
        float step = rotationSpeed * Time.deltaTime;
        float newYRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetYRotation, step);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);

        if (Mathf.Approximately(newYRotation, targetYRotation))
        {
            StartCoroutine(PauseAndSwitchDirection());
        }
    }

    IEnumerator PauseAndSwitchDirection()
    {
        isPaused = true;
        yield return new WaitForSeconds(2.0f);
        rotatingLeft = !rotatingLeft;
        SetTargetYRotation();
        isPaused = false;
    }

    private void SetTargetYRotation()
    {
        targetYRotation = rotatingLeft ? startYRotation - rotationAngle : startYRotation + rotationAngle;
    }

    public void SetPlayerSpotted(bool value)
    {
        this.playerIsSpotted = value;
    }
}
