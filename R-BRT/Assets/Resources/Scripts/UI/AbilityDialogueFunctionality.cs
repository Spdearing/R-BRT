using System.Collections;
using UnityEngine;
using TMPro;

public class AbilityDialogueFunctionality : MonoBehaviour
{
    [Header("TMP_Text")]
    [SerializeField] private TMP_Text uiText;

    [Header("Strings")]
    [SerializeField] private string[] fullStealthAbilityText;
    [SerializeField] private string[] fullJetPackAbilityText;
    [SerializeField] private string[] fullStealthAbilityText2;
    [SerializeField] private string[] fullJetPackAbilityText2;
    [SerializeField] private string[] fullStealthAbilityText3;
    [SerializeField] private string[] fullJetPackAbilityText3;

    [Header("Floats")]
    [SerializeField] private float delay;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SceneActivity sceneActivity;

    [Header("GameObject")]
    [SerializeField] private GameObject continueText;
    [SerializeField] private GameObject firstDialogue;

    private bool skipToNextText;
    private bool isTyping;
    private bool textFullyDisplayed;

    [Header("Dialogue CheckPoint Name")]
    [SerializeField] private string dialogueName;

    private string[][] dialogues;

    [SerializeField] private AudioSource textSound;

    private void Initialize()
    {
        dialogueName = string.Empty;
        dialogues = new string[][] { fullStealthAbilityText, fullJetPackAbilityText, fullStealthAbilityText2, fullJetPackAbilityText2, fullStealthAbilityText3, fullJetPackAbilityText3 };
        playerController = GameManager.instance.ReturnPlayerController();
        sceneActivity = GameManager.instance.ReturnSceneActivity();
    }

    private void StartDialogue()
    {
        dialogueName = GameManager.instance.ReturnPlayerDialogueAbilityChoice();
        string[] currentDialogue = GetCurrentDialogue(dialogueName);

        if (currentDialogue != null)
        {
            StartCoroutine(ShowDialogue(currentDialogue));
        }
    }

    private string[] GetCurrentDialogue(string dialogueName)
    {
        switch (dialogueName)
        {
            case "Stealth":
                return dialogues[0];
            case "Jetpack":
                return dialogues[1];
            case "Stealth2":
                return dialogues[2];
            case "Jetpack2":
                return dialogues[3];
            case "Stealth3":
                return dialogues[4];
            case "Jetpack3":
                return dialogues[5];
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
            isTyping = true;
            skipToNextText = false;
            textFullyDisplayed = false;

            for (int i = 0; i <= fullText.Length; i++)
            {
                if (skipToNextText)
                {
                    uiText.text = fullText;
                    break;
                }

                uiText.text = fullText.Substring(0, i);
                yield return new WaitForSeconds(delay);
                float randomPitchAmount = Random.Range(1.0f, 1.25f);
                textSound.pitch = randomPitchAmount;
                textSound.Play();
            }

            isTyping = false;
            textFullyDisplayed = true;
            skipToNextText = false;

            continueText.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        EndDialogue();
    }

    private void EndDialogue()
    {
        isTyping = false;
        textFullyDisplayed = false;
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        sceneActivity.SetDialogueAbilityTrigger(false);
    }

    private void OnEnable()
    {
        firstDialogue = GameManager.instance.ReturnDialogue();
        if (firstDialogue.activeSelf == true)
        {
            firstDialogue.SetActive(false);
        }
        else
        {
            firstDialogue.SetActive(false);
        }
        textSound = gameObject.GetComponent<AudioSource>();

        delay = 0.10f;
        Initialize();
        StartDialogue();
    }

    public void SetDialogue(string desiredDialogue)
    {
        dialogueName = desiredDialogue;
    }

    IEnumerator CountDownTillClose()
    {
        yield return new WaitForSeconds(10.0f);
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        sceneActivity.SetDialogueAbilityTrigger(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                skipToNextText = true;
            }
            else if (textFullyDisplayed)
            {
                textFullyDisplayed = false;
            }
        }
    }
}
