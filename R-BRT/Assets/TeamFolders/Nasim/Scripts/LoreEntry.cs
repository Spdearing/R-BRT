using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoreEntry : MonoBehaviour
{
	[Header("Game Object")]
	[SerializeField] GameObject loreEntryMenu;

	private void Start()
	{

		loreEntryMenu = GameManager.instance.ReturnLoreEntryMenu(); 
        Time.timeScale = 1.0f;
        loreEntryMenu.SetActive(false);
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

    public void PickupLoreEntry()
    {
        loreEntryMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
