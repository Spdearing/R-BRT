using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Assignment/Lab/Project: R-BRT
// Name: Nasim Issa
// Section: SGD285.4171
// Instructor: Aurore Locklear
// Date: 04/20/2024

public class PauseMenu : MonoBehaviour
{

    [Header("Game Objects")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject entriesPanel;
    [SerializeField] private GameObject fuelMeter;
    [SerializeField] private GameObject invisbilityMeter;

    [Header("Bools")]
    [SerializeField] private bool isPaused;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;

    Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        optionsPanel.SetActive(false);
        entriesPanel.SetActive(false);
        panels.Add("PauseMenuPanel", pauseMenu);
        panels.Add("OptionsPanel", optionsPanel);
        panels.Add("EntriesPanel", entriesPanel);

        playerController = GameManager.instance.ReturnPlayerController();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.ReturnPlayerActivity() == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                    
                }
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        EnableCursor();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        DisableCursor();
    }

    public void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
    public void SwitchToOptionsPanel()
    {
        SwitchPanel("OptionsPanel");
    }

    public void SwitchToEntriesPanel()
    {
        SwitchPanel("EntriesPanel");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void BackToMainPanel()
    {
        SwitchPanel("PauseMenuPanel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}