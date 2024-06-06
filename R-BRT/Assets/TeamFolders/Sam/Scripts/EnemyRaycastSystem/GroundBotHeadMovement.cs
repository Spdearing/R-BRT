using System.Collections;
using UnityEngine;

public class GroundBotHeadMovement : MonoBehaviour
{
    bool robotIsActive = true;
    bool playerIsSpotted;
    bool rotatingLeft = true;
    bool isPaused = false;

    [SerializeField] private float rotationAngle = 45f; // Rotation angle in degrees
    [SerializeField] private float rotationSpeed = 25f; // Rotation speed
    [SerializeField] private GameObject groundBotHead;
    [SerializeField] private Renderer groundBotHeadColor;


    private float startYRotation;
    private float targetYRotation;

    // Start is called before the first frame update
    void Start()
    {
        groundBotHead = gameObject.transform.Find("Head").gameObject;
        groundBotHeadColor = groundBotHead.GetComponent<Renderer>();
        groundBotHeadColor.material.color = Color.green; // Change light color to red
        startYRotation = transform.eulerAngles.y;
        SetTargetYRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if (robotIsActive && !isPaused && !playerIsSpotted)
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
        playerIsSpotted = value;
        if (playerIsSpotted)
        {
            groundBotHeadColor.material.color = Color.red; // Change light color to red
        }
        else
        {
            groundBotHeadColor.material.color = Color.red; // Change light color to red
        }
    }
}
