using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class GroundBotStateMachine : MonoBehaviour
{
    [Header("LayerMask")]
    [SerializeField] LayerMask groundMask;

    [Header("GameObjects")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject endingPoint;

    [Header("Vector3")]
    [SerializeField] Vector3 startingLocation;

    [Header("Floats")]
    [SerializeField] float robotHeight = 1.329f;

    [Header("Rigidbody")]
    private Rigidbody rb;

    [Header("Bools")]
    [SerializeField] bool isGrounded;


    public BehaviourState currentState;

    public enum BehaviourState
    {
        patrolling,
        chasing,
        scanning,
        reset
        
    }

    // Start is called before the first frame update
    void Start()
    {
        startingLocation = transform.position;
        currentState = BehaviourState.patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, robotHeight * .5f, groundMask);
    }

    public void ChangeState(BehaviourState newState)
    {
        currentState = newState;
    }
}
