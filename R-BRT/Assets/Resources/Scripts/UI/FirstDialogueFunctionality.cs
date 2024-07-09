using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstDialogueFunctionality : MonoBehaviour
{
    [Header("TMP_Text")]
    [SerializeField] private TMP_Text uiText;

    [Header("Strings")]
    [SerializeField] private string[] fullTexts1;
    [SerializeField] private string[] fullTexts2;
    [SerializeField] private string[] fullTexts3;
    [SerializeField] private string[] fullTexts4;

    [Header("Floats")]
    [SerializeField] private float delay = 0.035f;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SceneActivity sceneActivity;

    [Header("GameObject")]
    [SerializeField] private GameObject continueText;

    [Header("Bools")]
    [SerializeField] private bool skipText;

    [Header("Dialogue CheckPoint Name")]
    [SerializeField] private string dialogueName;

    [Header("Index for Dialogue")]
    [SerializeField] private int dialogueIndex = 0;

    private string[][] dialogues;



    void Start()
    {
        dialogueName = GameManager.instance.ReturnPlayerDialogueCheckPoints(dialogueIndex);
        Debug.Log(dialogueIndex);
        playerController = GameManager.instance.ReturnPlayerController();
        sceneActivity = GameManager.instance.ReturnSceneActivity();

        dialogues = new string[][] { fullTexts1, fullTexts2, fullTexts3, fullTexts4 };

        if (dialogueName == "First Dialogue")
        {
            StartCoroutine(ShowDialogue(dialogues[0]));
        }
        else if (dialogueName == "Second Dialogue")
        {
            StartCoroutine(ShowDialogue(dialogues[1]));
        }
        if (dialogueName == "Third Dialogue")
        {
            StartCoroutine(ShowDialogue(dialogues[2]));
        }
        else if (dialogueName == "Fourth Dialogue")
        {
            StartCoroutine(ShowDialogue(dialogues[3]));
        }


    }

    private IEnumerator ShowDialogue(string[] texts)
    {
        for (int j = 0; j < texts.Length; j++)
        {
            continueText.SetActive(false);
            uiText.text = "";
            string fullText = texts[j];

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
        skipText = false;
        dialogueName = GameManager.instance.ReturnPlayerDialogueCheckPoints(dialogueIndex++);
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        sceneActivity.SetDialogueTriggerOne(false);
    }

    private void OnEnable()
    {
        dialogueName = GameManager.instance.ReturnPlayerDialogueCheckPoints(dialogueIndex);

        dialogues = new string[][] { fullTexts1, fullTexts2, fullTexts3, fullTexts4 };

        if (dialogueName == "First Dialogue")
        {
            StartCoroutine(ShowDialogue(dialogues[0]));
        }
        else if (dialogueName == "Second Dialogue")
        {
            StartCoroutine(ShowDialogue(dialogues[1]));
        }
        if (dialogueName == "Third Dialogue")
        {
            StartCoroutine(ShowDialogue(dialogues[2]));
        }
        else if (dialogueName == "Fourth Dialogue")
        {
            StartCoroutine(ShowDialogue(dialogues[3]));
        }
    }

    public void UpdateDialogue()
    {
        dialogueName = GameManager.instance.ReturnDialogueCheckPoint();


    }
}
