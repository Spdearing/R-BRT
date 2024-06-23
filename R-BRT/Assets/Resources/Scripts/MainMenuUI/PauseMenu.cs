using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Assignment/Lab/Project: Portal
// Name: Nasim Issa
// Section: SGD285.4171
// Instructor: Aurore Locklear
// Date: 04/20/2024

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject screenPrompt;
    [SerializeField] bool isPaused;

    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject objectivePanel;
    [SerializeField] GameObject itemsPanel;
    [SerializeField] GameObject pauseMenuPanel;

    private GameManager gameManager;

    Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        panels.Add("PauseMenuPanel", pauseMenuPanel);
        panels.Add("OptionsPanel", optionsPanel);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        crosshair.SetActive(false);
        screenPrompt.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        EnableCursor();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        crosshair.SetActive(true);
        screenPrompt.SetActive(true);
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
    public void SwitchToHelpPanel()
    {
        SwitchPanel("OptionsPanel");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void BackToMainPanel()
    {
        SwitchPanel("PauseMenuPanel");
    }
}