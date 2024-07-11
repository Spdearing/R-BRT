using System.Collections;
using UnityEngine;
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
    [SerializeField] private string[] fullTexts5;
    [SerializeField] private string[] fullTexts6;
    [SerializeField] private string[] fullTexts7;
    [SerializeField] private string[] fullTexts8;

    [Header("Floats")]
    [SerializeField] private float delay;

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
        Initialize();
        StartDialogue();
    }

    private void Initialize()
    {
        dialogues = new string[][] { fullTexts1, fullTexts2, fullTexts3, fullTexts4, fullTexts5, fullTexts6, fullTexts7, fullTexts8};
        playerController = GameManager.instance.ReturnPlayerController();
        sceneActivity = GameManager.instance.ReturnSceneActivity();
        delay = 0.035f;
    }

    private void StartDialogue()
    {
        dialogueName = GameManager.instance.ReturnPlayerDialogueCheckPoints(dialogueIndex);
        string[] currentDialogue = GetCurrentDialogue(dialogueName, GetDialogues());

        if (currentDialogue != null)
        {
            StartCoroutine(ShowDialogue(currentDialogue));
        }
    }

    private string[][] GetDialogues()
    {
        return dialogues;
    }

    private string[] GetCurrentDialogue(string dialogueName, string[][] dialogues)
    {
        switch (dialogueName)
        {
            case "First Dialogue":
                return dialogues[0];
            case "Second Dialogue":
                return dialogues[1];
            case "Third Dialogue":
                return dialogues[2];
            case "Fourth Dialogue":
                return dialogues[3];
            case "Fifth Dialogue":
                return dialogues[4];
            case "Sixth Dialogue":
                return dialogues[5];
            case "Seventh Dialogue":
                return dialogues[6];
            case "Eighth Dialogue":
                return dialogues[7];
            default:
                return null;
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
                if (Input.GetMouseButtonDown(0))
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
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        EndDialogue();
    }

    private void EndDialogue()
    {
        skipText = false;
        dialogueName = GameManager.instance.ReturnPlayerDialogueCheckPoints(dialogueIndex++);
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        sceneActivity.SetDialogueTriggerOne(false);
    }

    private void OnEnable()
    {
        StartDialogue();

        if(dialogueIndex > 0) 
        {
            StartCoroutine(CountDownTillClose());
        }
    }

    IEnumerator CountDownTillClose()
    {
        yield return new WaitForSeconds(10.0f);
        dialogueName = GameManager.instance.ReturnPlayerDialogueCheckPoints(dialogueIndex++);
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        sceneActivity.SetDialogueTriggerOne(false);
    }

    public void UpdateDialogue()
    {
        dialogueName = GameManager.instance.ReturnDialogueCheckPoint();
        StartDialogue();
    }
}
