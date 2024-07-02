using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstDialogueFunctionality : MonoBehaviour
{
    [Header("TMP_Text")]
    [SerializeField] private TMP_Text uiText;

    [Header("Strings")]
    [SerializeField] private string[] fullTexts;
    [SerializeField] private string fullText;
    [SerializeField] private string fullText2;
    [SerializeField] private string fullText3;
    [SerializeField] private string fullText4;
    [SerializeField] private string fullText5;

    [Header("Floats")]
    [SerializeField] private float delay = 0.035f;

    [Header("Scripts")]
    [SerializeField] private GameManager gameManger;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SceneActivity sceneActivity;

    [Header("GameObject")]
    [SerializeField] private GameObject continueText;

    private bool skipText;

    void Start()
    {
        // Initialize GameManager and PlayerController references if not set in Inspector
        if (gameManger == null)
        {
            gameManger = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }

        if (playerController == null)
        {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }
        if (sceneActivity == null)
        {
            sceneActivity = GameObject.FindWithTag("Canvas").GetComponent<SceneActivity>();
        }

        // Initialize fullTexts array with provided text
        fullTexts = new string[5];
        fullTexts[0] = fullText;
        fullTexts[1] = fullText2;
        fullTexts[2] = fullText3;
        fullTexts[3] = fullText4;
        fullTexts[4] = fullText5;

        // Start the dialogue coroutine
        StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue()
    {
        for (int j = 0; j < fullTexts.Length; j++)
        {
            continueText.SetActive(false);
            uiText.text = "";
            string fullText = fullTexts[j];

            skipText = false;
            for (int i = 0; i <= fullText.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    skipText = true;
                }

                if (skipText)
                {
                    uiText.text = fullText;
                    break;
                }
                else
                {
                    uiText.text = fullText.Substring(0, i);
                    yield return new WaitForSeconds(delay);
                }
            }

            continueText.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        }

        sceneActivity.SetDialogueTriggerOne(false);
        sceneActivity.SetFirstDialogueHit(true);
        playerController.SetCameraLock(false);
        playerController.SetPlayerActivity(true);
    }
}