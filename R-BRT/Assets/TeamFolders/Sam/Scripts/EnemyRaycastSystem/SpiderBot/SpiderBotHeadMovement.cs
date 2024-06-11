using System.Collections;
using UnityEngine;

public class SpiderBotHeadMovement : MonoBehaviour
{

    [SerializeField] private bool playerIsSpotted;
    

    [Header("Floats")]
    [SerializeField] private float rotationAngle;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float startYRotation;
    [SerializeField] private float targetYRotation;

    [Header("Game Objects")]
    [SerializeField] private GameObject spiderBotHead;


    [Header("Scripts")]
    [SerializeField] private EnemyFieldOfView enemyFieldOfView;


    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetPlayerSpotted(bool value)
    {
        this.playerIsSpotted = value;
    }
}
