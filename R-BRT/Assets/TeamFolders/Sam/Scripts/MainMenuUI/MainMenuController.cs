using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Assignment/Lab/Project: Portal
// Name: Samuel Dearing
// Section: SGD285.4171
// Instructor: Aurore Locklear
// Date: 05/5/2024

public class MainMenuController : MonoBehaviour
{
 
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject playMenuPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject creditsPanel;


    Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        panels.Add("MainMenuPanel", mainMenuPanel);
        panels.Add("PlayMenuPanel", playMenuPanel);
        panels.Add("OptionsPanel", optionsPanel);
        panels.Add("CreditsPanel", creditsPanel);

        SwitchPanel("MainMenuPanel");
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

    public void SwitchToHelpPanel()
    {
        SwitchPanel("OptionsPanel");
    }

    public void SwitchToCreditsPanel()
    {
        SwitchPanel("CreditsPanel");
    }

    public void SwitchToPlayMenu()
    {
        SwitchPanel("PlayMenuPanel");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}