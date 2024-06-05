using System.Collections;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    bool robotIsActive = true;
    bool playerIsSpotted;
    bool rotatingLeft = true;
    bool isPaused = false;

    [SerializeField] private float rotationAngle = 45f; // Rotation angle in degrees
    [SerializeField] private float rotationSpeed = 25f; // Rotation speed
    [SerializeField] Light enemyLight;

    private float startYRotation;
    private float targetYRotation;

    // Start is called before the first frame update
    void Start()
    {
        startYRotation = transform.eulerAngles.y;
        SetTargetYRotation();
        enemyLight = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (robotIsActive && !isPaused && !playerIsSpotted)
        {
            enemyLight.color = Color.green;

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
            enemyLight.color = Color.red; // Change the light color to red if the player is spotted
        }
        else
        {
            enemyLight.color = Color.green; // Change the light color to green if the player is not spotted
        }
    }
}
