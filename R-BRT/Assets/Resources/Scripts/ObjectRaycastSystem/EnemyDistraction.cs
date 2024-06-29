using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistraction : MonoBehaviour
{
    [Header("Floats")]
    [SerializeField] private float expansionRate;
    [SerializeField] private float maxRadius;
    [SerializeField] private float initialRadius;

    [Header("Colliders")]
    [SerializeField] private SphereCollider sphereCollider;

    [Header("Bools")]
    [SerializeField] private bool isExpanding;

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

    [Header("Audio Sources")]
    [SerializeField] private AudioSource rockHit;

    private void Start()
    {
        if (sphereCollider == null)
        {
            sphereCollider = gameObject.GetComponent<SphereCollider>();
        }

        isExpanding = false;
        expansionRate = 30.0f;
        maxRadius = 10.0f;
        initialRadius = 0.15f;
        sphereCollider.isTrigger = true; // Ensure the collider is a trigger
        sphereCollider.radius = initialRadius;

        if (throwObject == null)
        {
            throwObject = GetComponent<ThrowObject>();
        }

        if (pickUpObject == null)
        {
            pickUpObject = GetComponent<PickUpObject>();
        }

        if (targetTransform == null)
        {
            targetTransform = gameObject.GetComponent<Transform>();
            

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isExpanding && throwObject.ReturnThrowStatus() && !other.gameObject.CompareTag("Player"))
        {
            isExpanding = true;
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
        rockHit.Play();
        while (isExpanding && sphereCollider.radius < maxRadius)
        {
            sphereCollider.radius += expansionRate * Time.deltaTime;

            // Check for enemies within the new radius
            Collider[] enemies = Physics.OverlapSphere(transform.position, sphereCollider.radius, ~ignoreLayerMask);

            foreach (Collider enemy in enemies)
            {
                if (IsTagDetectable(enemy.tag))
                {
                    Debug.Log("Detected enemy: " + enemy.name);

                    if (enemy.tag == "GroundBot")
                    {
                        groundBotHeadMovement = enemy.gameObject.GetComponent<GroundBotHeadMovement>();

                        if (groundBotHeadMovement != null)
                        {
                            groundBotHeadMovement.SetPlayerIsDistracted(true);
                            StartCoroutine(EnemyLookAtForDuration(enemy.transform, targetTransform.position, 3.0f));
                        }
                    }
                    StartCoroutine(EnemyLookAtForDuration(enemy.transform, targetTransform.position, 3.0f));
                }
            }

            yield return null;
        }

        ResetExpansion();
    }

    private IEnumerator EnemyLookAtForDuration(Transform enemy, Vector3 targetPosition, float duration)
    {
        float timeElapsed = 0f;
        Quaternion initialRotation = enemy.rotation;
        Vector3 direction = (targetPosition - enemy.position).normalized;
        direction.y = 0; // Ensure the enemy looks horizontally
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Rotate to target
        while (timeElapsed < duration)
        {
            enemy.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        enemy.rotation = targetRotation; // Ensure the rotation is set to the target rotation at the end

        // Wait at the target for 3 seconds
        yield return new WaitForSeconds(3.0f);

        timeElapsed = 0f; // Reset timeElapsed for returning to the initial rotation

        // Rotate back to initial rotation
        while (timeElapsed < duration)
        {
            enemy.rotation = Quaternion.Lerp(targetRotation, initialRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        enemy.rotation = initialRotation; // Ensure the rotation is set back to the initial rotation at the end

        if (groundBotHeadMovement != null)
        {
            groundBotHeadMovement.SetPlayerIsDistracted(false);
        }
        else
        {
            Debug.Log("No GroundBotHeadMovement script found on " + enemy.name);
        }
    }

    private void ResetExpansion()
    {
        sphereCollider.radius = initialRadius;
        isExpanding = false;
        Debug.Log("Expansion reset");
    }
}
