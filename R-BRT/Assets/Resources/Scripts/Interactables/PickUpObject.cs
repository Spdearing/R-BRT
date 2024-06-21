using System.Collections;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{

    [Header("Bools")]
    [SerializeField] private bool holding;
    [SerializeField] private bool pickingUp;

    [Header("Rigid Body")]
    [SerializeField] private Rigidbody rb;

    [Header("Transform")]
    [SerializeField] private Transform holdPosition;

    [Header("Coroutine")]
    private Coroutine moveCoroutine;

    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "PickUpItem";
        holdPosition = GameObject.Find("HoldPosition").GetComponent<Transform>();
        holding = false;
    }

    void Update()
    {
        if (pickingUp || holding)
        {
            if (moveCoroutine == null)
            {
                float moveDuration = pickingUp ? 0.5f : 0.05f;
                moveCoroutine = StartCoroutine(MoveObjectSmoothly(holdPosition.position, moveDuration));
                playerAnimator.SetBool("holdingRock", true);

            }
        }
    }

    public void PickUp()
    {
        pickingUp = true;
        rb.isKinematic = true;
        playerAnimator.SetBool("pickingUpRock", true);

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveObjectSmoothly(holdPosition.position, 0.25f));
    }

    public void PutDown()
    {
        holding = false;
        pickingUp = false;
        rb.isKinematic = false;
        playerAnimator.SetBool("holdingRock", false);

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    IEnumerator MoveObjectSmoothly(Vector3 end, float duration)
    {
        Vector3 start = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        if (pickingUp)
        {
            pickingUp = false;
            holding = true;
        }

        moveCoroutine = null;
    }

    public GameObject ReturnThisObject()
    {
        return this.gameObject;
    }

    public bool ReturnHoldingStatus()
    {
        return this.holding;
    }
}
