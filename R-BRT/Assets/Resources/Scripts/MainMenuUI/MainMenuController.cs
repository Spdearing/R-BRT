using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
 
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject introPanel;
    [SerializeField] bool mainMenuOpen;
    [SerializeField] bool introPanelOpen;




    Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        mainMenuOpen = true;
        panels.Add("MainMenuPanel", mainMenuPanel);
        panels.Add("InstructionsPanel", instructionsPanel);
        panels.Add("OptionsPanel", optionsPanel);
        panels.Add("CreditsPanel", creditsPanel);
        panels.Add("IntroPanel", introPanel);

        SwitchPanel("MainMenuPanel");
    }

    private void Update()
    {
        PlayGame();
    }


    public void SwitchPanel(string panelName)
    {
        string currentPanelName;

        foreach (var panel in panels)
        {
            if (panel.Key == panelName)
            {
                panel.Value.SetActive(true);
                currentPanelName = panelName;
            }
            else
            {
                panel.Value.SetActive(false);
            }
        }
    }

    public void BackToMainMenu()
    {
        SwitchPanel("MainMenuPanel");
    }

    public void SwitchToOptionsPanel()
    {
        SwitchPanel("OptionsPanel");
    }

    public void SwitchToCreditsPanel()
    {
        SwitchPanel("CreditsPanel");
    }
    public void SwitchToinstuctionsPanel()
    {
        SwitchPanel("InstructionsPanel");
    }

    public void StartIntro()
    {
        SwitchPanel("IntroPanel");
        introPanelOpen = true;
    }
    public void PlayGame()
    {
        if(introPanelOpen == true && Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("GameScene");
        } 
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public bool ReturnMainMenuOpen()
    {
        return this.mainMenuOpen;
    }



}