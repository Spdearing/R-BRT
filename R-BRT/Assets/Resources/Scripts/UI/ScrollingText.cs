using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Use this if you are using TextMeshPro
using UnityEngine.SceneManagement;

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
        Time.timeScale = 1.0f;
        if (textRectTransform == null)
        {
            textRectTransform = GetComponent<RectTransform>();
        }

        StartCoroutine(StartScrolling());
        HideSkipText();
        Invoke("ShowSkipText", 1);
    }

    private void Update()
    {
        GoBackToMainMenu();
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

            textRectTransform.anchoredPosition += new Vector2(-currentScrollSpeed * Time.deltaTime, 0 );
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

    public void GoBackToMainMenu()
    {
        if(skipText.text == "[Press Left - Click To Skip]" && Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
