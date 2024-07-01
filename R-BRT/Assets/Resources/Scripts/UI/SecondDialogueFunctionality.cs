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
    [SerializeField] private float delay = 0.035f;

    [Header("Scripts")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController playerController;

    [Header("GameObject")]
    [SerializeField] private GameObject continueText;

    private bool skipText;
    private bool canSkip;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        fullTexts = new string[2] { fullText, fullText2 };
        canSkip = true; // Initially, skipping is allowed
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
                if (canSkip && Input.GetKeyDown(KeyCode.Return))
                {
                    skipText = true;
                    canSkip = false; // Disable skipping temporarily
                    StartCoroutine(EnableSkippingAfterDelay(0.5f)); // Re-enable skipping after 0.5 seconds
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

        EndDialogue();
    }

    private IEnumerator EnableSkippingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSkip = true;
    }

    private void EndDialogue()
    {
        gameManager.SetDialogueTriggerTwo(false);
        gameManager.SetDialogueTwoHit(true);
        gameManager.TurnOffSecondDialogueHitBox();
        playerController.SetCameraLock(false);
        playerController.SetPlayerActivity(true);
    }
}

