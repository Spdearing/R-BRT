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


    [SerializeField] private string[] fullTexts2;
    [SerializeField] private string fullSecondText;
    [SerializeField] private string fullSecondText2;
    [SerializeField] private string fullSecondText3;
    [SerializeField] private string fullSecondText4;
    [SerializeField] private string fullSecondText5;

    [SerializeField] private string[] fullTexts3;
    [SerializeField] private string fullThirdText;
    [SerializeField] private string fullThirdText2;
    [SerializeField] private string fullThirdText3;
    [SerializeField] private string fullThirdText4;
    [SerializeField] private string fullThirdText5;

    [SerializeField] private string[] fullTexts4;
    [SerializeField] private string fullFourthText;
    [SerializeField] private string fullFourthText2;
    [SerializeField] private string fullFourthText3;
    [SerializeField] private string fullFourthText4;
    [SerializeField] private string fullFourthText5;

    [Header("Floats")]
    [SerializeField] private float delay = 0.035f;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SceneActivity sceneActivity;

    [Header("GameObject")]
    [SerializeField] private GameObject continueText;

    [Header("Bools")]
    [SerializeField] private bool skipText;
    [SerializeField] private bool firstDialogue = true;
    [SerializeField] private bool secondDialogue = false;
    [SerializeField] private bool thirdDialogue = false;
    [SerializeField] private bool fourthDialogue = false;


    void Start()
    {
        playerController = GameManager.instance.ReturnPlayerController();
        sceneActivity = GameManager.instance.ReturnSceneActivity();

        fullTexts = new string[5];
        fullTexts[0] = fullText;
        fullTexts[1] = fullText2;
        fullTexts[2] = fullText3;
        fullTexts[3] = fullText4;
        fullTexts[4] = fullText5;

        fullTexts2 = new string[5];
        fullTexts[0] = fullSecondText;
        fullTexts[1] = fullSecondText2;
        fullTexts[2] = fullSecondText3;
        fullTexts[3] = fullSecondText4;
        fullTexts[4] = fullSecondText5;

        fullTexts3 = new string[5];
        fullTexts[0] = fullThirdText;
        fullTexts[1] = fullThirdText2;
        fullTexts[2] = fullThirdText3;
        fullTexts[3] = fullThirdText4;
        fullTexts[4] = fullThirdText5;

        fullTexts4 = new string[5];
        fullTexts[0] = fullFourthText;
        fullTexts[1] = fullFourthText2;
        fullTexts[2] = fullFourthText3;
        fullTexts[3] = fullFourthText4;
        fullTexts[4] = fullFourthText5;

        

        if (firstDialogue)
        {
            StartCoroutine(ShowFirstDialogue());
        }
        else if(secondDialogue)
        {
            ShowSecondDialogue();
        }
        if (thirdDialogue)
        {
            ShowThirdDialogue();
        }
        else if (fourthDialogue)
        {
            ShowFourthDialogue();
        }

    }

    private IEnumerator ShowFirstDialogue()
    {
        for (int j = 0; j < fullTexts2.Length; j++)
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
    }

    private IEnumerator ShowSecondDialogue()
    {
        for (int j = 0; j < fullTexts2.Length; j++)
        {
            continueText.SetActive(false);
            uiText.text = "";
            string fullText = fullTexts2[j];

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
    }

    private IEnumerator ShowThirdDialogue()
    {
        for (int j = 0; j < fullTexts3.Length; j++)
        {
            continueText.SetActive(false);
            uiText.text = "";
            string fullText = fullTexts3[j];

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
    }

    private IEnumerator ShowFourthDialogue()
    {
        for (int j = 0; j < fullTexts4.Length; j++)
        {
            continueText.SetActive(false);
            uiText.text = "";
            string fullText = fullTexts4[j];

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
    }

    public void SetFirstDialgue(bool value)
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