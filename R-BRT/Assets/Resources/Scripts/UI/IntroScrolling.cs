using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroScrolling : MonoBehaviour
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
    }

    private void Update()
    {
        if (isScrolling)
        {
            if (textRectTransform.offsetMax.y >= 40)
            {
                Debug.Log("Showing Text");
                ShowSkipText();
            }

            if (Input.GetMouseButton(0))
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
        textRectTransform.anchoredPosition = new Vector2(textRectTransform.anchoredPosition.x, Screen.height);
        HideSkipText();
        SceneManager.LoadScene("GameScene");
    }
}
