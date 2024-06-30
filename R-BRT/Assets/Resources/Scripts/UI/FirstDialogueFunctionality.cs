using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstDialogueFunctionality : MonoBehaviour
{
    [Header("TMP_Text")]
    [SerializeField] private TMP_Text uiText;

    [Header("Strings")]
    [SerializeField] private string currentText = "";
    [SerializeField] private string[] fullTexts;
    [SerializeField] private string fullText;
    [SerializeField] private string fullText2;
    [SerializeField] private string fullText3;
    [SerializeField] private string fullText4;
    [SerializeField] private string fullText5;

    [Header("Floats")]
    [SerializeField] private float delay;


    // Start is called before the first frame update
    void Start()
    {
        fullTexts = new string[5];
        fullTexts[0] = fullText;
        fullTexts[1] = fullText2;
        fullTexts[2] = fullText3;
        fullTexts[3] = fullText4;
        fullTexts[4] = fullText5;
        delay = .035f;
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        for (int j = 0; j < fullTexts.Length; j++)
        {
            currentText = "";
            for (int i = 0; i <= fullTexts[j].Length; i++)
            {
                currentText = fullTexts[j].Substring(0, i);
                uiText.text = currentText;
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(1.0f); // Wait before showing the next dialogue
        }
    }
}
