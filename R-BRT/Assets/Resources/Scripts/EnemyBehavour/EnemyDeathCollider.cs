using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathCollider : MonoBehaviour
{

    [SerializeField] GroundBotStateMachine groundBotStateMachine;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerAbilities playerAbilities;


    private void Start()
    {
        groundBotStateMachine = transform.parent.parent.GetComponent<GroundBotStateMachine>();
        playerController = GameManager.instance.ReturnPlayerController();
        playerAbilities = GameManager.instance.ReturnPlayerAbilities();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathBox" && playerAbilities.ReturnUsingInvisibility() == false)
        {
            groundBotStateMachine.ChangeBehavior(GroundBotStateMachine.BehaviorState.playerCaught);
            playerController.SetPlayerActivity(false);
        }
    }

    
}
