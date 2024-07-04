using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;



public class PhoenixChipDecision : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] GameObject phoenixChipMenu;




    private void Start()
    {
        Debug.Log(" UI is popping");
        phoenixChipMenu = GameManager.instance.ReturnPhoenixChipMenu(); 
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

    public void PlayerDecision()
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
