using System.Collections;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private bool holding;
    private bool pickingUp;
    private Rigidbody rb;

    [SerializeField] private Transform holdPosition;

    private Coroutine moveCoroutine;

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
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            float moveDuration = pickingUp ? 0.5f : 0.05f;

            moveCoroutine = StartCoroutine(MoveObjectSmoothly(this.gameObject.transform.position, holdPosition.transform.position, moveDuration));
        }
    }

    public void PickUp()
    {
        pickingUp = true;
        rb.isKinematic = true;
        Holding();
    }

    void Holding()
    {
        pickingUp = false;
        holding = true;
    }

    public void PutDown()
    {
        holding = false;
        rb.isKinematic = false;
    }

    IEnumerator MoveObjectSmoothly(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(start, end, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = end;
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
