using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecondDialogueFunctionality : MonoBehaviour
{
    [Header("TMP_Text")]
    [SerializeField] private TMP_Text uiText;

    [Header("Strings")]
    [SerializeField] private string currentText = "";
    [SerializeField] private string[] fullTexts;
    [SerializeField] private string fullText;
    [SerializeField] private string fullText2;
    

    [Header("Floats")]
    [SerializeField] private float delay;

    [Header("Scripts")]
    [SerializeField] private GameManager gameManger;
    [SerializeField] private PlayerController playerController;

    [Header("GameObject")]
    [SerializeField] private GameObject continueText;


    // Start is called before the first frame update
    void Start()
    {
        gameManger = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        fullTexts = new string[2];
        fullTexts[0] = fullText;
        fullTexts[1] = fullText2;
        delay = .035f;
        StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue()
    {
        for (int j = 0; j < fullTexts.Length; j++)
        {
            continueText.SetActive(false);
            uiText.text = "";
            string fullText = fullTexts[j];

            for (int i = 0; i <= fullText.Length; i++)
            {
                uiText.text = fullText.Substring(0, i);
                yield return new WaitForSeconds(delay);
            }
            continueText.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        }
        gameManger.SetDialogueTriggerOne(false);
        playerController.SetCameraLock(false);
        playerController.SetPlayerActivity(true);
    }
}
