using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThirdDialogueFunctionality : MonoBehaviour
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
    [SerializeField] private SceneActivity sceneActivity;
    [SerializeField] private PlayerController playerController;

    [Header("GameObject")]
    [SerializeField] private GameObject continueText;

    private bool skipText;

    // Start is called before the first frame update
    void Start()
    {
        sceneActivity = GameManager.instance.ReturnSceneActivity();
        playerController = GameManager.instance.ReturnPlayerController();
        fullTexts = new string[2] { fullText, fullText2 };
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

        EndDialogue();
    }

    private void EndDialogue()
    {
        sceneActivity.SetDialogueTriggerThree(false);
        sceneActivity.SetDialogueThreeHit(true);
        sceneActivity.TurnOffThirdDialogueHitBox();
    }
}
