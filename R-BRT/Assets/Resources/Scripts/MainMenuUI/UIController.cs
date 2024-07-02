using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;



public class UIController : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] GameObject phoenixChipMenu;




    private void Start()
    {
        phoenixChipMenu = GameObject.FindWithTag("PhoenixChipMenu"); 
        Time.timeScale = 1.0f;
        phoenixChipMenu.SetActive(false);
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

    public void PhoenixChipDecision()
    {
        phoenixChipMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ChooseYourFriend()
    {
        SceneManager.LoadScene("VictorySamLives");
    }

    public void SaveTheWorld()
    {
        SceneManager.LoadScene("SamDies");
    }
}
