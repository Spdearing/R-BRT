using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    bool robotIsActive;
    bool playerIsSpotted;

    [SerializeField] private Vector3 turningHeadLeft;
    [SerializeField] private Vector3 turningHeadRight;

    [SerializeField] Light enemyLight;


    private bool rotatingLeft = true;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
        turningHeadLeft = new Vector3(0, 25, 0); // Turning the head clockwise
        turningHeadRight = new Vector3(0, -25, 0); // Turning the head counterclockwise
        robotIsActive = true;
        enemyLight = GetComponentInChildren<Light>();

        StartCoroutine(MovingHeadRotation());
    }

    // Update is called once per frame
    void Update()
    {
        if (robotIsActive && !isPaused && !playerIsSpotted)
        {
            enemyLight.color = Color.green;
            if (rotatingLeft)
            {
                transform.Rotate(turningHeadLeft * Time.deltaTime);

                if (transform.eulerAngles.y >= 45 && transform.eulerAngles.y <= 60)
                {
                    StartCoroutine(PauseAndSwitchDirection());
                }
            }
            else
            {
                transform.Rotate(turningHeadRight * Time.deltaTime);

                if (transform.eulerAngles.y <= 315 && transform.eulerAngles.y >= 300)
                {
                    StartCoroutine(PauseAndSwitchDirection());
                }
            }
        }
    }

    IEnumerator MovingHeadRotation()
    {
        while (robotIsActive)
        {
            yield return null;
        }
    }

    IEnumerator PauseAndSwitchDirection()
    {
        isPaused = true;
        yield return new WaitForSeconds(2.0f);
        rotatingLeft = !rotatingLeft;
        isPaused = false;
    }

    public void SetPlayerSpotted(bool value)
    {
        playerIsSpotted = value;
    }
}



