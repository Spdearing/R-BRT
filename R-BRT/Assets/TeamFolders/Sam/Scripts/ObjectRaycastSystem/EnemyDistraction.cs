using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingSphere : MonoBehaviour
{
    [Header("Floats")]
    [SerializeField] private float expansionRate;
    [SerializeField] private float maxRadius;

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


    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true; // Ensure the collider is a trigger
        maxRadius = 10.0f;
        expansionRate = 20.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isExpanding)
        {
            // Start expanding the sphere
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
                    EnemyLookAt(enemy.transform, targetTransform);
                }
            }

            yield return null;
        }
        isExpanding = false;
    }

    private void EnemyLookAt(Transform enemy, Transform target)
    {
        Vector3 direction = (target.position - enemy.position).normalized;
        direction.y = 0; // Optional: Keep the enemy on the same horizontal plane
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        enemy.rotation = Quaternion.Slerp(enemy.rotation, lookRotation, Time.deltaTime * 5.0f); // Adjust the rotation speed if needed
    }
}
