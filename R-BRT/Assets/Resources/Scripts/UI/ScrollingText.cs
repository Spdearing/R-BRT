using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Use this if you are using TextMeshPro

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed = 60.0f;
    public float startDelay = 1.0f;
    public float speedUpMultiplier = 4.0f; // Factor by which to increase the speed when holding space
    public RectTransform textRectTransform;
    public TMP_Text skipText;
    private bool isScrolling = true;

    private void OnEnable()
    {
        if (textRectTransform == null)
        {
            textRectTransform = GetComponent<RectTransform>();
        }

        StartCoroutine(StartScrolling());
        HideSkipText();
        Invoke("ShowSkipText", 10);
    }

    private void Update()
    {
        if (isScrolling)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SkipScrolling();
            }
        }
    }

    IEnumerator StartScrolling()
    {
     
        yield return new WaitForSeconds(startDelay);

     
        while (textRectTransform.anchoredPosition.y < Screen.height)
        {
            float currentScrollSpeed = scrollSpeed;

 
            if (Input.GetKey(KeyCode.Space))
            {
                currentScrollSpeed *= speedUpMultiplier;
            }

            textRectTransform.anchoredPosition += new Vector2(0, currentScrollSpeed * Time.deltaTime);
            yield return null;
        }

        // End of scrolling
        isScrolling = false;
        HideSkipText();
    }

    void ShowSkipText()
    {
        if (skipText != null)
        {
            skipText.text = "[Press Left - Click To Skip]";
        }
    }

    void HideSkipText()
    {
        if (skipText != null)
        {
            skipText.text = "";
        }
    }

    void SkipScrolling()
    {
        isScrolling = false;
        //StopAllCoroutines();
        //textRectTransform.anchoredPosition = new Vector2(textRectTransform.anchoredPosition.x, Screen.height);
        HideSkipText();
    }
}
