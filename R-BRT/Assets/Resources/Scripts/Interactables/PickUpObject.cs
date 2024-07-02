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

    [Header("Audio Source")]
    [SerializeField] private AudioSource rockHolding;

    void Start()
    {
        Debug.Log("Pickup is popping");
        rb = gameObject.GetComponent<Rigidbody>();
        gameObject.tag = "PickUpItem";
        holdPosition = GameObject.Find("HoldPosition").GetComponent<Transform>();
        playerAnimator = GameObject.FindWithTag("Body").GetComponent<Animator>();
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
        rockHolding.Play();
    }

    public void PutDown()
    {
        holding = false;
        pickingUp = false;
        rb.isKinematic = false;
        playerAnimator.SetBool("holdingRock", false);
        playerAnimator.SetBool("pickingUpRock", false);

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        rockHolding.Stop();
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
