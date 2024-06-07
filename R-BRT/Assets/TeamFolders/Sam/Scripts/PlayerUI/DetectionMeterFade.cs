using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.0f; // Duration of the fade
    [SerializeField] private Image border;
    [SerializeField] private Image detectionAmount;
    [SerializeField] private Image detectionEmpty;

    private void Awake()
    {
        if (border == null)
        {
            border = transform.Find("Border").GetComponent<Image>();
        }
        if (detectionAmount == null)
        {
            detectionAmount = transform.Find("DetectionAmount").GetComponent<Image>();
        }
        if (detectionEmpty == null)
        {
            detectionEmpty = transform.Find("DetectionEmpty").GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private void OnDisable()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color borderColor = border.color;
        Color detectionAmountColor = detectionAmount.color;
        Color detectionEmptyColor = detectionEmpty.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            borderColor.a = alpha;
            detectionAmountColor.a = alpha;
            detectionEmptyColor.a = alpha;

            border.color = borderColor;
            detectionAmount.color = detectionAmountColor;
            detectionEmpty.color = detectionEmptyColor;

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color borderColor = border.color;
        Color detectionAmountColor = detectionAmount.color;
        Color detectionEmptyColor = detectionEmpty.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - elapsedTime / fadeDuration);

            borderColor.a = alpha;
            detectionAmountColor.a = alpha;
            detectionEmptyColor.a = alpha;

            border.color = borderColor;
            detectionAmount.color = detectionAmountColor;
            detectionEmpty.color = detectionEmptyColor;

            yield return null;
        }
    }
}
