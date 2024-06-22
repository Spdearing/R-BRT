using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistraction : MonoBehaviour
{
    [Header("Floats")]
    [SerializeField] private float expansionRate = 30.0f;
    [SerializeField] private float maxRadius = 10.0f;
    [SerializeField] private float initialRadius = 0.5f;

    [Header("Colliders")]
    [SerializeField] private SphereCollider sphereCollider;

    [Header("Bools")]
    [SerializeField] private bool isExpanding = false;

    [Header("Transforms")]
    [SerializeField] private Transform targetTransform;

    [Header("LayerMask")]
    [SerializeField] private LayerMask ignoreLayerMask;

    [Header("Tags")]
    [SerializeField] private string[] enemyTags;

    [Header("Scripts")]
    [SerializeField] private ThrowObject throwObject;
    [SerializeField] private PickUpObject pickUpObject;
    [SerializeField] private GroundBotHeadMovement groundBotHeadMovement;

    private void Start()
    {
        if (sphereCollider == null)
        {
            sphereCollider = GetComponent<SphereCollider>();
        }
        sphereCollider.isTrigger = true; // Ensure the collider is a trigger
        sphereCollider.radius = initialRadius;

        throwObject = GetComponent<ThrowObject>();
        pickUpObject = GetComponent<PickUpObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isExpanding && throwObject.ReturnThrowStatus() == true && !other.CompareTag("Player"))
        {
            Debug.Log("Trigger entered and expanding sphere collider");

            isExpanding = true;
            targetTransform = other.transform;
            StartCoroutine(ExpandSphere());
        }
    }

    private bool IsTagDetectable(string tag)
    {
        foreach (string detectableTag in enemyTags)
        {
            if (detectableTag == tag)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator ExpandSphere()
    {
        while (isExpanding && sphereCollider.radius < maxRadius)
        {
            sphereCollider.radius += expansionRate * Time.deltaTime;

            // Check for enemies within the new radius
            Collider[] enemies = Physics.OverlapSphere(transform.position, sphereCollider.radius, ~ignoreLayerMask);

            foreach (Collider enemy in enemies)
            {
                if (IsTagDetectable(enemy.tag))
                {
                    if (enemy.tag == "GroundBot")
                    {
                        groundBotHeadMovement = enemy.gameObject.GetComponent<GroundBotHeadMovement>();
                        groundBotHeadMovement.SetPlayerSpotted(true);
                    }

                    StartCoroutine(EnemyLookAtForDuration(enemy.transform, targetTransform.position, 5.0f));
                }
            }

            yield return null;
        }

        ResetExpansion();
    }

    private IEnumerator EnemyLookAtForDuration(Transform enemy, Vector3 targetPosition, float duration)
    {
        float timeElapsed = 0f;
        Vector3 direction = (targetPosition - enemy.position).normalized;
        direction.y = 0; // Ensure the enemy looks horizontally
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        while (timeElapsed < duration)
        {
            enemy.rotation = Quaternion.Slerp(enemy.rotation, lookRotation, Time.deltaTime * 2.5f);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (groundBotHeadMovement != null)
        {
            groundBotHeadMovement.SetPlayerSpotted(false);
        }
    }

    private void ResetExpansion()
    {
        sphereCollider.radius = initialRadius;
        isExpanding = false;
        Debug.Log("Expansion reset");
    }
}
