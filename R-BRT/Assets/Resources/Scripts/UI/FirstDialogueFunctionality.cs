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
    [SerializeField] private bool firstDialogue;
    [SerializeField] private bool secondDialogue;
    [SerializeField] private bool thirdDialogue;
    [SerializeField] private bool fourthDialogue;

    private string[][] dialogues;


    public void Awake()
    {
        firstDialogue = true;
        secondDialogue = false;
        thirdDialogue = false;
        fourthDialogue = false;
    }

    void Start()
    {
        playerController = GameManager.instance.ReturnPlayerController();
        sceneActivity = GameManager.instance.ReturnSceneActivity();

        dialogues = new string[][] { fullTexts1, fullTexts2, fullTexts3, fullTexts4 };

        if (firstDialogue)
        {
            StartCoroutine(ShowDialogue(dialogues[0]));
        }
        else if (secondDialogue)
        {
            StartCoroutine(ShowDialogue(dialogues[1]));
        }
        else if (thirdDialogue)
        {
            StartCoroutine(ShowDialogue(dialogues[2]));
        }
        else if (fourthDialogue)
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
        firstDialogue = false;
        playerController.SetPlayerActivity(true);
        playerController.isCameraLocked = false;
        sceneActivity.SetDialogueTriggerOne(false);
        //sceneActivity.SetFirstDialogueHit(true);
    }

    public void SetFirstDialogue(bool value)
    {
        firstDialogue = value;
    }

    public void SetSecondDialogue(bool value)
    {
        secondDialogue = value;
    }

    public void SetThirdDialogue(bool value)
    {
        thirdDialogue = value;
    }

    public void SetFourthDialogue(bool value)
    {
        fourthDialogue = value;
    }
}
