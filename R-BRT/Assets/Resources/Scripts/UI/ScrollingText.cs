using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Use this if you are using TextMeshPro

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed = 20.0f;
    public float startDelay = 2.0f; 

    [SerializeField] private RectTransform textRectTransform;

    private void OnEnable()
    {
        
        textRectTransform = GetComponent<RectTransform>();

        
        StartCoroutine(StartScrolling());
    }

    IEnumerator StartScrolling()
    {
        
        yield return new WaitForSeconds(startDelay);

        
        while (textRectTransform.anchoredPosition.y < Screen.height)
        {
            
            textRectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
            yield return null;
        }
    }
}