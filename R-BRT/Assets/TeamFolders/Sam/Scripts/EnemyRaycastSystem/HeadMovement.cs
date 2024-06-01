using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    bool playerIsAlive;

    [SerializeField] private Vector3 turningHeadLeft;
    [SerializeField] private Vector3 turningHeadRight;
    [SerializeField] private Vector3 lookingHeadUpSpeed;
    [SerializeField] private Vector3 lookingHeadDownSpeed;


    // Start is called before the first frame update
    void Start()
    {
        turningHeadLeft = new Vector3(0, 25, 0); // Turning the head clockwise
        turningHeadRight = new Vector3(0, -25, 0); // Turning the head clockwise
        lookingHeadUpSpeed = new Vector3(0, 100, 0); // Moving the head up 
        lookingHeadDownSpeed = new Vector3(0, -100, 0); // Moving the head up
        //playerIsAlive = true;
        StartCoroutine(MovingHeadRotation());
    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator MovingHeadRotation()
    {

        transform.Rotate(turningHeadLeft * Time.deltaTime);

        if (transform.eulerAngles.x == 150)
        {
            yield return new WaitForSeconds(4.0f);
            transform.Rotate(turningHeadRight * Time.deltaTime);
        }

    }


}


