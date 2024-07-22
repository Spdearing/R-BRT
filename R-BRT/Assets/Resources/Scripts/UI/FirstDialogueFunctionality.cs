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
    [SerializeField] private string checkPointDialogue;

    [Header("Floats")]
    [SerializeField] private float delay;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SceneActivity sceneActivity;

    [Header("GameObject")]
    [SerializeField] private GameObject continueText;

    private bool skipToNextText;
    private bool isTyping;
    private bool textFullyDisplayed;

    [Header("Dialogue CheckPoint Name")]
    [SerializeField] private string dialogueName;

    [Header("Index for Dialogue")]
    [SerializeField] private int dialogueIndex = 0;

    private string[][] dialogues;
    private Coroutine currentDialogueCoroutine;

    [SerializeField] private AudioSource textSound;

    void Start()
    {
        textSound = gameObject.GetComponent<AudioSource>();
        checkPointDialogue = "First Dialogue";
        Initialize();
        StartDialogue();
    }

    private void Initialize()
    {
        dialogues = new string[][] { fullTexts1, fullTexts2, fullTexts3, fullTexts4, fullTexts5, fullTexts6, fullTexts7, fullTexts8 };
        playerController = GameManager.instance.ReturnPlayerController();
        sceneActivity = GameManager.instance.ReturnSceneActivity();
    }

    private void StartDialogue()
    {
        dialogueName = checkPointDialogue;
        string[] currentDialogue = GetCurrentDialogue(dialogueName);

        if (currentDialogue != null)
        {
            if (currentDialogueCoroutine != null)
            {
                StopCoroutine(currentDialogueCoroutine);
            }
            currentDialogueCoroutine = StartCoroutine(ShowDialogue(currentDialogue));
        }
    }

    private string[] GetCurrentDialogue(string dialogueName)
    {
        switch (dialogueName)
        {
            case "First Dialogue": return dialogues[0];
            case "Second Dialogue": return dialogues[1];
            case "Third Dialogue": return dialogues[2];
            case "Fourth Dialogue": return dialogues[3];
            case "Fifth Dialogue": return dialogues[4];
            case "Sixth Dialogue": return dialogues[5];
            case "Seventh Dialogue": return dialogues[6];
            case "Eighth Dialogue": return dialogues[7];
            default: return null;
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
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        sceneActivity.SetDialogueTriggerOne(false);
    }

    private void OnEnable()
    {
        delay = 0.10f;
        StartDialogue();
    }

    public void SetDialogue(string desiredDialogue)
    {
        checkPointDialogue = desiredDialogue;
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
                skipToNextText = true;
                textFullyDisplayed = false;
            }
        }
    }
}
