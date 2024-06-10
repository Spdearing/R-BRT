using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;



public class UIController : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject phoenixChipMenu;


    [Header("Bool")]
    private bool isPaused;

    private void Start()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        isPaused = false;
        phoenixChipMenu.SetActive(false);
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
        isPaused = false;
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


    public void PhoenixChipDecision()
    {
        phoenixChipMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ChooseYourFriend()
    {
        SceneManager.LoadScene("ChooseYourFriend");
    }

    public void SaveTheWorld()
    {
        SceneManager.LoadScene("SaveTheWorld");
    }
}
