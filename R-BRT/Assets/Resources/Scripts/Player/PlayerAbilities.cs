using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    [Header("bools")]
    [SerializeField] private bool invisibilityUnlocked;
    [SerializeField] private bool invisibilityAvailable;
    [SerializeField] private bool jetPackUnlocked;
    [SerializeField] private bool invisibilityMeterFillingBackUp;
    [SerializeField] private bool usingInvisibility;


    [Header("Invisibility")]
    [SerializeField] private Image invisibleMeter;
    [SerializeField] float maxInvisible;
    [SerializeField] float invisibleIncrement;
    [SerializeField] float startingInvisible;

    [Header("Scripts")]
    [SerializeField] private Jetpack jetPack;
    [SerializeField] private InvisibilityCloak invisibilityCloak;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource activatingInvisibility;
    [SerializeField] private AudioSource invisiblityDuration;

    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;

    [Header("Images")]
    [SerializeField] private Image invisibilityVisualMeter;
    [SerializeField] private Image invisibilityVisualEmpty;
    [SerializeField] private Image invisibilityVisualAmount;

    void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (jetPackUnlocked)
        {
            jetPack.enabled = true;
        }

        HandleInvisibility();
        InvisibilityMeter();
        InvisibilityMeterFillingBackUp();
    }


    void Setup()
    {
        invisibilityCloak = GameManager.instance.ReturnInvisibilityCloak();
        jetPack = gameObject.GetComponent<Jetpack>();
        playerAnimator = GameManager.instance.ReturnAnimator();
        invisibilityVisualMeter = GameManager.instance.ReturnInvisibilityMeterImage();
        invisibilityVisualEmpty = GameManager.instance.ReturnInvisibilityMeterEmpty();
        invisibilityVisualAmount = GameManager.instance.ReturnInvisibilityMeterAmount();
        invisibleMeter = GameManager.instance.ReturnInvisibilityMeterAmount();
        activatingInvisibility = GameManager.instance.ReturnActivatingInvisibilitySound();
        invisiblityDuration = GameManager.instance.ReturnInvisibilityDurationSound();
        usingInvisibility = false;
        invisibilityAvailable = true;
        jetPackUnlocked = false;
        invisibilityUnlocked = false;
        maxInvisible = 7.5f;
        invisibleIncrement = .25f;
        startingInvisible = 7.5f;
        invisibilityMeterFillingBackUp = false;
        
    }

    public void HandleInvisibility()
    {
        if (Input.GetKeyDown(KeyCode.Q) && invisibilityAvailable && invisibilityUnlocked && !invisibilityMeterFillingBackUp)
        {
            gameObject.tag = "Invisible";
            playerAnimator.SetTrigger("usingInvisibility 0");
            GameManager.instance.ReturnInvisibilityVolume().SetActive(true);
            StartCoroutine(InvisibilityTimer());
        }
    }

    private IEnumerator InvisibilityTimer()
    {
        yield return new WaitForSeconds(.875f);
        activatingInvisibility.Play();
        invisibilityCloak.TurnInvisible();
        yield return new WaitForSeconds(.1f);
        invisiblityDuration.Play();

        usingInvisibility = true;
        invisibilityAvailable = false;
        yield return new WaitForSeconds(6.0f);
        invisiblityDuration.Stop();
        invisibilityCloak.TurnVisible();
    }

    public void InvisibilityMeter()
    {
        if (!invisibilityAvailable)
        {

            startingInvisible -= 5.0f * Time.deltaTime * invisibleIncrement;
            startingInvisible = Mathf.Clamp(startingInvisible, 0, maxInvisible);
            invisibleMeter.fillAmount = startingInvisible / maxInvisible;

            if (startingInvisible <= 0)
            {
                startingInvisible = 0;
                invisibilityAvailable = true;
                GameManager.instance.ReturnInvisibilityVolume().SetActive(false);
                usingInvisibility = false;
                invisibilityMeterFillingBackUp = true;
                gameObject.tag = "Player";
            }
        }

    }

    public void HideInvisibilityMeter()
    {
        Color amountColor = invisibilityVisualAmount.color;
        amountColor.a = 0.0f;
        invisibilityVisualAmount.color = amountColor;

        Color emptyColor = invisibilityVisualEmpty.color;
        emptyColor.a = 0.0f;
        invisibilityVisualEmpty.color = emptyColor;

        Color meterColor = invisibilityVisualMeter.color;
        meterColor.a = 0.0f;
        invisibilityVisualMeter.color = meterColor;
    }

    public void DisplayInvisibilityMeter()
    {
        Color amountColor = invisibilityVisualAmount.color;
        amountColor.a = 255.0f;
        invisibilityVisualAmount.color = amountColor;

        Color emptyColor = invisibilityVisualEmpty.color;
        emptyColor.a = 255.0f;
        invisibilityVisualEmpty.color = emptyColor;

        Color meterColor = invisibilityVisualMeter.color;
        meterColor.a = 255.0f;
        invisibilityVisualMeter.color = meterColor;
    }

    public void InvisibilityMeterFillingBackUp()
    {
        if (invisibilityMeterFillingBackUp)
        {
            startingInvisible += 2.5f * Time.deltaTime * invisibleIncrement;
            startingInvisible = Mathf.Clamp(startingInvisible, 0, maxInvisible);
            invisibleMeter.fillAmount = startingInvisible / maxInvisible;

            if (startingInvisible >= 7.5f)
            {
                startingInvisible = 7.5f;
                invisibilityAvailable = true;
                invisibilityMeterFillingBackUp = false;
            }
        }

    }

    public bool ReturnInvisibilityStatus()
    {
        return this.invisibilityAvailable;
    }

    public void SetJetPackUnlock(bool value)
    {
        jetPackUnlocked = value;
    }

    public void SetInvisibilityUnlock(bool value)
    {
        invisibilityUnlocked = value;
    }

    public float ReturnStartingInvisibility()
    {
        return this.startingInvisible;
    }
    public float ReturnMaxInvisibility()
    {
        return this.maxInvisible;
    }

    public Image ReturnInvisibilityMeter()
    {
        return this.invisibleMeter;
    }

    public bool ReturnUsingInvisibility()
    {
        return this.usingInvisibility;
    }

}
