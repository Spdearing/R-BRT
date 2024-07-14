using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameObject samPanel;
    [SerializeField] GameObject annaPanel;
    [SerializeField] GameObject nasimPanel;
    [SerializeField] GameObject colbyPanel;
    [SerializeField] GameObject tylerPanel;
    [SerializeField] GameObject rayPanel;

    [SerializeField] Button previousPanelButton;
    [SerializeField] Button nextPanelButton;


    Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();

    [Header("List")]
    [SerializeField] private List<string> panelKeys = new List<string>();

    [Header("Int")]
    [SerializeField] private int currentPanelIndex = 0;

    void Start()
    {
        panels.Add("SamPanel", samPanel);
        panels.Add("AnnaPanel", annaPanel);
        panels.Add("NasimPanel", nasimPanel);
        panels.Add("ColbyPanel", colbyPanel);
        panels.Add("TylerPanel", tylerPanel);
        panels.Add("RayPanel", rayPanel);

        panelKeys.Add("AnnaPanel");
        panelKeys.Add("ColbyPanel");
        panelKeys.Add("NasimPanel");
        panelKeys.Add("RayPanel");
        panelKeys.Add("SamPanel");
        panelKeys.Add("TylerPanel");

        SwitchPanel("AnnaPanel");
    }

    private void Update()
    {
        DisableButtons();
    }

    private void OnEnable()
    {
        previousPanelButton = GameObject.Find("PreviousPanel").GetComponent<Button>();
        nextPanelButton = GameObject.Find("NextPanel").GetComponent<Button>();
    }


    public void SwitchPanel(string panelName)
    {
        foreach (var panel in panels)
        {
            panel.Value.SetActive(panel.Key == panelName);
        }
    }

    public void NextPanel()
    {
        currentPanelIndex = (currentPanelIndex + 1) % panelKeys.Count;
        SwitchPanel(panelKeys[currentPanelIndex]);
    }

    public void PreviousPanel()
    {
        currentPanelIndex = (currentPanelIndex - 1) % panelKeys.Count;
        SwitchPanel(panelKeys[currentPanelIndex]);
    }

    void DisableButtons()
    {
        if (currentPanelIndex == 0)
        {
            previousPanelButton.interactable = false;
        }
        else if (currentPanelIndex == 5)
        {
            nextPanelButton.interactable = false;
        }
        else
        {
            previousPanelButton.interactable = true;
            nextPanelButton.interactable = true;
        }

    }
}