using System.Collections;
using UnityEngine;

public class GroundBotHeadMovement : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] private bool robotIsActive;
    [SerializeField] private bool playerIsSpotted;
    [SerializeField] private bool rotatingLeft;
    [SerializeField] private bool isPaused;

    [Header("Floats")]
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationSpeed;
    private float startYRotation;
    private float targetYRotation;

    [Header("Transform")]
    [SerializeField] private Transform player;

    [Header("Game Objects")]
    [SerializeField] private GameObject groundBotHead;
    [SerializeField] private GameObject fieldOfView;

    [Header("Renderer")]
    [SerializeField] private Renderer groundBotHeadColor;
    [SerializeField] private Renderer fieldOfViewRenderer;

    [Header("Scripts")]
    [SerializeField] private EnemyFieldOfView enemyFieldOfView;
    [SerializeField] private GameManager gameManager;

    [Header("Materials")]
    [SerializeField] private Material yellow;
    [SerializeField] private Material lightBlue;
    [SerializeField] private Material red;
    [SerializeField] private Material fieldOfViewLightBlue;
    [SerializeField] private Material fieldOfViewYellow;
    [SerializeField] private Material fieldOfViewRed;

    void Start()
    {
        isPaused = false;
        rotatingLeft = true;
        robotIsActive = true;
        rotationAngle = 45f;
        rotationSpeed = 25f;
        fieldOfViewRenderer = GameObject.FindWithTag("FOV").GetComponentInChildren<Renderer>();
        groundBotHeadColor.material = lightBlue;
        fieldOfViewRenderer.material = lightBlue;
        startYRotation = transform.eulerAngles.y;
        SetTargetYRotation();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (robotIsActive && !isPaused)
        {
            if (playerIsSpotted)
            {
                RotateTowardsPlayer();
                groundBotHeadColor.material = yellow;
                fieldOfViewRenderer.material = yellow;
            }
            else
            {
                Patrol();
                groundBotHeadColor.material = lightBlue;
                fieldOfViewRenderer.material = lightBlue;
            }
        }
    }

    private void Patrol()
    {
        float step = rotationSpeed * Time.deltaTime;
        float newYRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetYRotation, step);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);

        if (Mathf.Approximately(newYRotation, targetYRotation))
        {
            StartCoroutine(PauseAndSwitchDirection());
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
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
        if (rotatingLeft)
        {
            targetYRotation = startYRotation - rotationAngle;
        }
        else
        {
            targetYRotation = startYRotation + rotationAngle;
        }
    }

    public void SetPlayerSpotted(bool value)
    {
        this.playerIsSpotted = value;
    }
}
