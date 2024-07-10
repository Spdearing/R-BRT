using System.Collections;
using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class AbilityDialogueFunctionality : MonoBehaviour
{
    [Header("TMP_Text")]
    [SerializeField] private TMP_Text uiText;

    [Header("Strings")]

    [SerializeField] private string[] fullStealthAbilityText;
    [SerializeField] private string[] fullJetPackAbilityText;

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
        dialogues = new string[][] { fullStealthAbilityText , fullJetPackAbilityText };
        playerController = GameManager.instance.ReturnPlayerController();
        sceneActivity = GameManager.instance.ReturnSceneActivity();
        delay = 0.035f;
    }

    private void StartDialogue()
    {
        dialogueName = GameManager.instance.ReturnPlayerDialogueAbilityChoice(dialogueIndex);
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
            case "Jetpack":
                return dialogues[0];
            case "Stealth":
                return dialogues[1];
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
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        sceneActivity.SetDialogueAbilityTrigger(false);
    }
}
