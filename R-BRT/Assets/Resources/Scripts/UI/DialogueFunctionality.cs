using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueFunctionality : MonoBehaviour
{
    [SerializeField] private Text uiText;
    [SerializeField] private string fullText;
    [SerializeField] private string fullText2;
    [SerializeField] private float delay;

    [SerializeField] private string currentText = "";

    // Start is called before the first frame update
    void Start()
    {
        delay = .035f;
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        for(int i = 0; i <= fullText.Length; i++) 
        {
            currentText = fullText.Substring(0, i);
            uiText.text = currentText;
            yield return new WaitForSeconds(delay);
        }
    }
}
