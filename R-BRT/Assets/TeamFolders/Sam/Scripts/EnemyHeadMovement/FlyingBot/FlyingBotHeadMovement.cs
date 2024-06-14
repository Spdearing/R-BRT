using System.Collections;
using UnityEngine;

public class FlyingBotHeadMovement : MonoBehaviour
{


    [Header("Bools")]
    [SerializeField] private bool robotIsActive;
    [SerializeField] private bool playerIsSpotted;
    [SerializeField] private bool rotatingLeft;
    [SerializeField] private bool isPaused;

    [Header("Floats")]
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float startYRotation;
    [SerializeField] private float targetYRotation;

    [Header("Game Objects")]
    [SerializeField] private GameObject flyingBotHead;

    [Header("Renderer")]
    [SerializeField] private Renderer flyingBotHeadColor;

    [Header("Scripts")]
    [SerializeField] private EnemyFieldOfView enemyFieldOfView;
    [SerializeField] private GameManager gameManager;

    [Header("Materials")]
    [SerializeField] private Material green;
    [SerializeField] private Material red;


    

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        rotatingLeft = true;
        robotIsActive = true;
        rotationAngle = 45f;
        rotationSpeed = 25f;
        flyingBotHeadColor.material = green; 
        startYRotation = transform.eulerAngles.y;
        SetTargetYRotation();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (robotIsActive && !isPaused && !playerIsSpotted)
        {
            if (gameManager.ReturnPlayerSpotted() == false)
            {
                float step = rotationSpeed * Time.deltaTime;

                float newYRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetYRotation, step);

                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);

                if (Mathf.Approximately(newYRotation, targetYRotation))
                {
                    StartCoroutine(PauseAndSwitchDirection());
                }
            }
        }
        //PlayerIsDetected();
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
