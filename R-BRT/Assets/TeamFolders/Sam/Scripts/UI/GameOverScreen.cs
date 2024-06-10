using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    [Header("Game Objects")]
    [SerializeField] GameObject gameOverPanel;


    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
    }


    public void ReloadCurrentScene()
    {
        
        Scene currentScene = SceneManager.GetActiveScene();

        
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1.0f;
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Time.timeScale = 1.0f;
    }

    public GameObject ReturnGameOverPanel()
    {
        return this.gameOverPanel;
    }

   
}
