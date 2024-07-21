using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoreEntryController : MonoBehaviour
{
    [SerializeField] GameObject loreEntryOne;
    [SerializeField] GameObject loreEntryTwo;
    [SerializeField] GameObject loreEntryThree;
    [SerializeField] GameObject loreEntryFour;


    Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        panels.Add("LoreEntryOne", loreEntryOne);
        panels.Add("LoreEntryTwo", loreEntryTwo);
        panels.Add("LoreEntryThree", loreEntryThree);
        panels.Add("LoreEntryFour", loreEntryFour);

        loreEntryOne.SetActive(false);
        loreEntryTwo.SetActive(false);
        loreEntryThree.SetActive(false);
        loreEntryFour.SetActive(false);
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


    public void OpenLoreOne()
    {
        GameManager.instance.StopBlinking();
        SwitchPanel("LoreEntryOne");
    }

    public void OpenLoreTwo()
    {
        GameManager.instance.StopBlinking();
        SwitchPanel("LoreEntryTwo");
    }
    public void OpenLoreThree()
    {
        GameManager.instance.StopBlinking();
        SwitchPanel("LoreEntryThree");
    }
    public void OpenLoreFour()
    {
        GameManager.instance.StopBlinking();
        SwitchPanel("LoreEntryFour");
    }
}
