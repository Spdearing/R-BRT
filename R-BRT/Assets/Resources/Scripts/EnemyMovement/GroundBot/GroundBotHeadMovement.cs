using System.Collections;
using UnityEngine;

public class GroundBotHeadMovement : MonoBehaviour
{
    [Header("Bools")]
    [SerializeField] private bool robotIsActive;
    [SerializeField] private bool playerIsSpotted;
    [SerializeField] private bool rotatingLeft;
    [SerializeField] private bool isPaused;
    [SerializeField] private bool isDistracted;

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
    [SerializeField] private EnemyGroundBotFieldOfView enemyGroundBotFieldOfView;
    [SerializeField] private GameManager gameManager;

    [Header("Materials")]
    [SerializeField] private Material red;
    [SerializeField] private Material fieldOfViewRed;

    void Start()
    {
        Debug.Log("GroundBotHeadMovement popping");
        isPaused = false;
        rotatingLeft = true;
        robotIsActive = true;
        rotationAngle = 45f;
        rotationSpeed = 25f;
        enemyGroundBotFieldOfView = groundBotHead.GetComponentInChildren<EnemyGroundBotFieldOfView>();
        fieldOfViewRenderer = GameObject.FindWithTag("FOV").GetComponentInChildren<Renderer>();
        groundBotHeadColor.material = red;
        fieldOfViewRenderer.material = fieldOfViewRed;
        startYRotation = transform.eulerAngles.y;
        SetTargetYRotation();
        player = GameManager.instance.ReturnPlayerTransform();
    }

    void Update()
    {
        if (robotIsActive && !isPaused)
        {
            if (playerIsSpotted && !isDistracted)
            {
                RotateTowardsPlayer();
            }
            else if(!isDistracted)
            {
                Patrol();
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

    public void SetPlayerIsDistracted(bool value)
    {
        this.isDistracted = value;
    }
}
