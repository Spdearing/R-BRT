using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] GameObject destinationA;
    [SerializeField] GameObject destinationB;


    [SerializeField] private SceneActivity sceneActivity;

    private void Start()
    {
        sceneActivity = GameManager.instance.ReturnSceneActivity();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Death")
        {
            transform.position = destinationA.transform.position;
        }

        else if (other.gameObject.tag == "SecondDialogueEncounter")
        {
            sceneActivity.StartSecondDialogue();
        }

        if (other.gameObject.tag == "ThirdDialogueEncounter")
        {
            sceneActivity.StartThirdDialogue();
        }
    }
}
