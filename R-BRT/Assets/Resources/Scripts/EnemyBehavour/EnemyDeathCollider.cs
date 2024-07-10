using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathCollider : MonoBehaviour
{

    [SerializeField] GroundBotStateMachine groundBotStateMachine;


    private void Start()
    {
        groundBotStateMachine = transform.parent.parent.GetComponent<GroundBotStateMachine>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathBox")
        {
            groundBotStateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.playerCaught);
            Debug.Log("hitting player");
        }
    }

    
}
