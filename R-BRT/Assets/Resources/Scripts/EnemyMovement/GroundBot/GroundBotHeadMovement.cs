using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

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
    [SerializeField] private Transform playerTransform;

    [Header("Game Objects")]
    [SerializeField] private GameObject groundBotHead;
    [SerializeField] private GameObject fieldOfView;


    [Header("Scripts")]
    [SerializeField] private EnemyGroundBotFieldOfView enemyGroundBotFieldOfView;
    [SerializeField] private EnemyProximityCheck enemyProximityCheck;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAbilities abilities;



    void Start()
    {
        Setup();
    }

    void Update()
    {
        if (playerIsSpotted && !isDistracted)
        {
            RotateTowardsPlayer();
        }
        else if (!isDistracted)
        {
            Patrol();
        }
        if(enemyProximityCheck.ReturnEnemyWithinRange() && playerController.PlayerIsRunning() == true && abilities.ReturnUsingInvisibility() == true)
        {
            RotateTowardsPlayer();
        }

    }

    void Setup()
    {
        playerIsSpotted = false;
        robotIsActive = true;
        isPaused = false;
        rotatingLeft = true;
        robotIsActive = true;
        isDistracted = false;
        rotationAngle = 45f;
        rotationSpeed = 50f;
        enemyGroundBotFieldOfView = groundBotHead.GetComponentInChildren<EnemyGroundBotFieldOfView>();
        playerController = GameManager.instance.ReturnPlayerController();
        enemyProximityCheck = GameManager.instance.ReturnEnemyProximityCheck();
        startYRotation = transform.eulerAngles.y;
        playerTransform = GameManager.instance.ReturnPlayerTransform();
        abilities = GameManager.instance.ReturnPlayerAbilities();
        SetTargetYRotation();
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

    public void RotateTowardsPlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

    IEnumerator PauseAndSwitchDirection()
    {
        if(!isPaused)
        {
            isPaused = true;
            yield return new WaitForSeconds(2.0f);
            rotatingLeft = !rotatingLeft;
            SetTargetYRotation();
            isPaused = false;
        }
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

    public bool ReturnPlayerIsSpotted()
    {
        return this.playerIsSpotted;
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
