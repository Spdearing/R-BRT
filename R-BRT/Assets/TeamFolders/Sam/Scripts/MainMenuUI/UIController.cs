using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;


// Assignment/Lab/Project: Portal
// Name: Nasim Issa
// Section: SGD285.4171
// Instructor: Aurore Locklear
// Date: 04/07/2024

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;

    private bool isPaused;

    private void Start()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
      if(Input.GetKeyUp(KeyCode.Escape)) 
        { 
            PauseGame();
        } 
    }

    void PauseGame()
    {
        TogglePauseMenu();
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            EnableCursor();
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            DisableCursor();
        }
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

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        DisableCursor();
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
