using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathCollider : MonoBehaviour
{

    [SerializeField] GroundBotStateMachine groundBotStateMachine;
    [SerializeField] PlayerController playerController;


    private void Start()
    {
        groundBotStateMachine = transform.parent.parent.GetComponent<GroundBotStateMachine>();
        playerController = GameManager.instance.ReturnPlayerController();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathBox")
        {
            groundBotStateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.playerCaught);
            playerController.SetPlayerActivity(false);
        }
    }

    
}
